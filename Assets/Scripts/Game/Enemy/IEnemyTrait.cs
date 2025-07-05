using UnityEngine;

public interface IEnemyTrait
{
    string traitName { get; set; }
    int priority { get; set; }
    float modifyDamage(float damage);
}
