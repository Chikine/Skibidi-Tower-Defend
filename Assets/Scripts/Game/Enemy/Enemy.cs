using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    //stat
    public float HP = 20f;
    public float speed = 1f;
    public float damage;
    public float buff = 0;
    public int moneyDrop = 10;
    //current type
    public bool isGround = true;
    public bool isFloating = false;
    //enemy type (trait)
    private Dictionary<string, bool> EnemyTypes = new();
    //effects
    public bool isSlow = false;
    public bool isBurn = false;
    public bool isRadioactive = false;
    private List<IEnemyTrait> enemyTraits = new();
    private Dictionary<string, Coroutine> activeEffects = new Dictionary<string, Coroutine>();
    //position
    public int[] currentPosition = new int[] {5,0};
    private int[] endPos = new int[] {5,23};
    Vector2 SpawnPoint = new(0.5f, -5.5f);
    //other 
    private Main main;
    private SpriteRenderer spriteRenderer;
    private HealthBar healthBar;
    private EnemyManager enemyManager;
    public void Start()
    {
        //other
        main = FindFirstObjectByType<Main>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyManager = FindFirstObjectByType<EnemyManager>();
        //update buff
        if(buff == 0)
        {
            buff = main.gameData.enemyBuff;
        }
        //health bar
        healthBar = GetComponentInChildren<HealthBar>();
        //trait
        enemyTraits.AddRange(GetComponents<IEnemyTrait>());
        enemyTraits.Sort((a,b) => a.priority - b.priority);
        foreach(IEnemyTrait enemyTrait in enemyTraits)
        {
            string enemyType = enemyTrait.traitName;
            EnemyTypes[enemyType] = true;
        }
        enemyManager.Register(this);
        transform.position = SpawnPoint;
    }

    private bool isSuccess = false;
    private bool isMoving = false;
    private Coroutine moveCoroutine = null;
    public void Update()
    {
        Color color = spriteRenderer.color;
        color.a = isTypeActivate("camo") ? 0.5F : 1f;
        spriteRenderer.color = color;
        if (HP <= 0)
        {
            die();
        }
        if (!isSuccess)
        {
            if (currentPosition[0] == endPos[0] && currentPosition[1] == endPos[1])
            {
                Debug.Log("succesfully attack player");
                main.gameData.HP -= 1 * buff;
                die();
                isSuccess = true;
                //handle winning position for enemy
            }
            else if(!isMoving) moveCoroutine = StartCoroutine(Move(1f));
        }
    }

    public void statBuff(float buff)
    {
        HP *= buff;
        speed *= buff;
        damage *= buff;
    }

    public void changePosition(int x, int y)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            isMoving = false;
        }
        currentPosition[0] = x;
        currentPosition[1] = y;
        Vector2 position = new(y + 0.5f, -x - 0.5f);
        transform.position = position;
    }

    public void SetSpeed(float spd)
    {
        speed = spd;
    }
    public void TakeDamage(float damage)
    {
        foreach(IEnemyTrait enemyTrait in enemyTraits)
        {
            if(isTypeActivate(enemyTrait.traitName))
            {
                damage = enemyTrait.modifyDamage(damage);
            }
        }
        HP -= damage;
        healthBar.ScaleUpdate();
        //Debug.Log(this.name + " HP: " + HP);
    }

    public bool isType(string type)
    {
        return EnemyTypes.ContainsKey(type);
    }

    public bool isTypeActivate(string type)
    {
        return EnemyTypes.TryGetValue(type, out bool isType)&& isType;
    }

    public void modifyType(string type, bool setBool)
    {
        if (isType(type))
        {
            Debug.Log($"Before modification: {type} = {EnemyTypes[type]}");
            EnemyTypes[type] = setBool;
            Debug.Log($"After modification: {type} = {EnemyTypes[type]}");
        }
    }

    public void die()
    {
        //clear list
        foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsSortMode.None))
        {
            tower.RemoveEnemy(gameObject);
        }
        //Debug.Log("enemy die");
        healthBar.enabled = false;
        enemyManager.Unregister(this);
        Destroy(gameObject);
        //give money to player
        GameData gameData = main.gameData;
        if(gameData != null)
        {
            int m = Mathf.Max(1, Mathf.RoundToInt(moneyDrop / Mathf.Sqrt(buff)));
            gameData.money += m;
            Debug.Log("player get " + m + " money");
        }
    }

    public IEnumerator Move(float duration)
    {
        isMoving = true;
        int[] nextPos = new int[2];
        GameData gameData = main.gameData;
        int minimumStep = gameData.path[currentPosition[0],currentPosition[1]];
        int row = gameData.height;
        int col = gameData.width;
        int x = currentPosition[0];
        int y = currentPosition[1];
        if (isFloating)
        {
            nextPos[0] = x;
            nextPos[1] = y + 1;
        }
        else
        {
            foreach (var (a, b) in new[] { (x, y + 1), (x, y - 1), (x + 1, y), (x - 1, y) })
            {
                //Debug.Log("try " + a + ", " + b);
                if (a >= 0 && a < row && b >= 0 && b < col)
                {
                    if (gameData.path[a, b] < minimumStep)
                    {
                        minimumStep = gameData.path[a, b];
                        nextPos[0] = a;
                        nextPos[1] = b;
                    }
                    //else if (gameData.path[a, b] == minimumStep && UnityEngine.Random.Range(0, 2) == 0)
                    //{
                    //    nextPos[0] = a;
                    //    nextPos[1] = b;
                    //}
                }
            }
        }
        //Debug.Log("move from " + currentPosition[0] + ", " + currentPosition[1] + " to " + nextPos[0] + ", " + nextPos[1]);
        Vector2 next = new(nextPos[1] + 0.5f, -nextPos[0] - 0.5f);
        yield return StartCoroutine(MoveAnim(next, duration));
        currentPosition = nextPos;
    }

    private IEnumerator MoveAnim(Vector2 next, float duration)
    {
        Vector2 start = transform.position;
        float len = Vector2.Distance(start, next);
        float startTime = Time.time;
        float currentSpeed = speed;
        spriteRenderer.flipX = start.x > next.x;
        while (Vector2.Distance(transform.position, next) > 0.01f)
        {
            if(speed != currentSpeed)
            {
                StartCoroutine(MoveAnim(next, duration));
                yield break;
            }
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / len;
            transform.position = Vector2.Lerp(start, next, fractionOfJourney);
            yield return null;
        }
        transform.position = next;
        isMoving = false;
    }

    public void ApplyEffect(string effect, Func<Enemy, string, float, IEnumerator> enumerator, float duration)
    {
        //Debug.Log("apply effect " + effect + " on " + this.name);
        //Debug.Log(HP);
        if (this != null && activeEffects.ContainsKey(effect) && activeEffects[effect] != null)
        {
            StopCoroutine(activeEffects[effect]);
            RemoveEffectKey(effect);
        }
        Coroutine coroutine = StartCoroutine(enumerator(this, effect, duration));
        activeEffects[effect] = coroutine;
    }    

    public bool HasEffect(string effect)
    {
        return activeEffects.ContainsKey(effect);
    }

    public void RemoveEffectKey(string effect)
    {
        if (activeEffects.ContainsKey(effect))
        {
            activeEffects.Remove(effect);
        }
    }
}