using System.Collections.Generic;
using UnityEngine;

public interface IProjectileAttack
{
    void Attack(List<Enemy> enemiesInRange, float damage);
}
