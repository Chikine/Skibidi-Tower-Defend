using UnityEngine;

public class freezerTrait : MonoBehaviour, IEnemyTrait
{
    public string traitName { get; set; } = "freezer";
    public int priority { get; set; } = 0;
    public float modifyDamage(float damage)
    {
        return damage / 1.2f;
    }
}
