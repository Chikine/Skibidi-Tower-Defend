using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tower : MonoBehaviour
{
    //other
    private float nextAttackTime = 0f;
    private Radius radius;
    private SpriteRenderer radiusSprite;
    private List<Enemy> enemiesInRange = new();
    private Animator anim;
    private Main main;
    private UpgradeMenu upgradeMenu;
    private List<ITowerAttack> attackStrategies = new();
    private Dictionary<string, Coroutine> preventTowerAttack = new();
    //Tower Position
    private int[] TowerPos = new int[2];
    //amount of animation video
    public int AmountOfAnim = 1;
    private int currentAnim = 0;
    //level: start level, max level
    public int level = 1;
    public int maxLevel = 4;
    //sprite for next level
    public Sprite NextLevelSprite;
    //prefab for managing
    public int prefabIndex;
    //range: default range, boolen update range
    public float range = 1.5f;
    //attack: max projectile live, attack rate, dmg
    public float attackCooldown = 1f;
    public float damage = 10f;
    //HP
    public float HP = 20f;
    //money issues
    public int upgradeCost;
    public int sellValue;
    //interact
    public bool isRotateTower;
    //status
    public bool canReachGround = true;
    public bool canReachSky = false;
    public bool isFreeze = false;
    public bool canSeeCamo = false;
    //status resist
    public bool resistFreeze = false;

    public void Start()
    {
        main = FindFirstObjectByType<Main>();
        upgradeMenu = FindFirstObjectByType<UpgradeMenu>();
        radius = GetComponentInChildren<Radius>();
        radiusSprite = radius.GetComponent<SpriteRenderer>();
        radius.setRadius(range);
        anim = GetComponent<Animator>();
        attackStrategies.AddRange(GetComponents<ITowerAttack>());
        TowerPos[0] = (int)-transform.position.y;
        TowerPos[1] = (int)transform.position.x;
        upgradeMenu.ShowMenu(this);
        //Debug.Log("Tower pos: " +  TowerPos[0] + ", " + TowerPos[1]);
    }

    public void Update()
    {
        if (enemiesInRange.Count > 0 && preventTowerAttack.Count == 0 && Time.time >= nextAttackTime && !isFreeze)
        {
            Attack();
        }
    }

    public void Attack()
    {
        List<Enemy> validTargets = new();
        foreach (Enemy enemy in enemiesInRange)
        {
            if (canAttackEnemy(enemy))
            {
                validTargets.Add(enemy);
            }
        }
        if (validTargets.Count == 0) return;
        //if found
        nextAttackTime = Time.time + attackCooldown;
        foreach (ITowerAttack attack in attackStrategies)
        {
            attack.Attack(validTargets , damage);
        }
        //Debug.Log("target get -" + damage + "hp, " + target.HP + "hp left");
    }
    public float offsetAngle = 0f;
    public void Rotate(Enemy target)
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + offsetAngle);
        }
    }

    public bool canAttackEnemy(Enemy enemy)
    {
        if(!canReachGround && enemy.isGround) return false;
        if(!canReachSky && enemy.isFloating) return false;
        if(!canSeeCamo && enemy.isTypeActivate("camo")) return false;
        return true;
    }
    public void AddEnemy(GameObject E)
    {
        Enemy enemy = E.GetComponent<Enemy>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject E)
    {
        Enemy enemy = E.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    public void OnMouseDown()
    {
        upgradeMenu.ShowMenu(this);
    }

    public void setRadiusOpacity(float opacity)
    {
        Color color = radiusSprite.color;

        color.a = opacity;
        radiusSprite.color = color;
    }

    public void Upgrade()
    {
        int nextPrefabIndex = prefabIndex + level;
        GameData gameData = main.gameData;
        gameData.money -= upgradeCost;
        gameData.arr[TowerPos[0], TowerPos[1]] = nextPrefabIndex;
        main.placeNewTower(transform.position, nextPrefabIndex);
        Debug.Log("upgrade " + gameObject.name);
        Destroy(gameObject);
    }
    public void Sell()
    {
        GameData gameData = main.gameData;
        gameData.money += sellValue;
        gameData.arr[TowerPos[0], TowerPos[1]] = 0;
        main.updatePath();
        Debug.Log("sell " + gameObject.name);
        Destroy(gameObject);
    }

    public Sprite nextSprite()
    {
        return NextLevelSprite;
    }

    public void addPreventAttackReason(string reason, float duration)
    {
        if (preventTowerAttack[reason] != null)
        {
            StopCoroutine(preventTowerAttack[reason]);
        }
        preventTowerAttack[reason] = StartCoroutine(applyPreventAttackReason(reason, duration));
    }

    private IEnumerator applyPreventAttackReason(string reason, float duration)
    {
        yield return new WaitForSeconds(duration);
        preventTowerAttack.Remove(reason);
    }
    public void PlayAnim()
    {
        if (anim == null) return;
        string name = level + "_" + currentAnim;
        anim.Play(name, 0, 0f);
        currentAnim = (currentAnim + 1) % AmountOfAnim;
    }
}