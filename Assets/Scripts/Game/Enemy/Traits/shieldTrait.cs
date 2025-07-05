using UnityEngine;

public class shieldTrait : MonoBehaviour, IEnemyTrait
{
    public string traitName { get; set; } = "shield"; 
    public int priority { get; set; } = -9999;
    private EnemyShield enemyShield;
    public void Start()
    {
        enemyShield = GetComponentInChildren<EnemyShield>();
    }
    public float modifyDamage(float damage)
    {
        if(enemyShield != null) return enemyShield.TakeDamage(damage);
        return damage;
    }
}
