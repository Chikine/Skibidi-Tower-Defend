using System.Collections;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private float shieldHP;
    private float currentShieldHP;
    public Sprite whenShield;
    public Sprite whenNotShield;
    public float recoverTime = 10f;
    public void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        shieldHP = enemy.HP;
        currentShieldHP = shieldHP;
    }
    public void Update()
    {
        spriteRenderer.sprite = (enemy.isTypeActivate("shield")) ? whenShield : whenNotShield;
    }

    public float TakeDamage(float damage)
    {
        if (!enemy.isTypeActivate("shield") || enemy == null)
        {
            return damage;
        }
        enemy.HP += damage;
        currentShieldHP -= damage;
        if (currentShieldHP < 0)
        {
            enemy.modifyType("shield", false);
            StartCoroutine(recoverShield());
        }
        return 0;
    }

    private IEnumerator recoverShield()
    {
        yield return new WaitForSeconds(recoverTime);
        if (enemy != null)
        {
            enemy.modifyType("shield", true);
            currentShieldHP = shieldHP;
        }
    }
}
