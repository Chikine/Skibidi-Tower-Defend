using System.Collections.Generic;
using UnityEngine;

public interface ITowerAttack
{
    void Attack(List<Enemy> enemiesInRange, float damage);
}

