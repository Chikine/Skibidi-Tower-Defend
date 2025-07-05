using UnityEngine;

public class metalTrait : MonoBehaviour, IEnemyTrait
{
    public string traitName { get; set; } = "metal";
    public int priority { get; set; } = 9999;
    public float modifyDamage(float damage)
    {
        return 0.01f;
    }
}
