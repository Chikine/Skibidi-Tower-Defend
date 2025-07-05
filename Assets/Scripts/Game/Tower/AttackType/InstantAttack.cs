using System.Collections.Generic;
using UnityEngine;

public class InstantAttack : MonoBehaviour, ITowerAttack
{
    private Tower tower;
    public bool AttackAll;

    public void Start()
    {
        tower = GetComponent<Tower>();
    }
    public void Attack(List<Enemy> enemiesInRange, float damage)
    {
        for (int i = 0; i < enemiesInRange.Count; ++i)
        {
            Enemy target = enemiesInRange[i];
            if (tower.isRotateTower)
            {
                tower.Rotate(enemiesInRange[i]);
            }
            if (target != null)
            {
                target.TakeDamage(damage);
                tower.PlayAnim();
            }
            if (!AttackAll) break;
        }
    }
}
