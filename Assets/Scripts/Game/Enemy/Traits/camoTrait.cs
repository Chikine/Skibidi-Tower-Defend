using UnityEngine;

public class camoTrait : MonoBehaviour, IEnemyTrait
{
    public string traitName { get; set; } = "camo";
    public int priority { get; set; } = 0;
    public float modifyDamage(float damage)
    {
        return damage;
    }
}
