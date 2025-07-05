using System.Collections;
using UnityEngine;

public class speedupTrait : MonoBehaviour, IEnemyTrait
{
    public string traitName { get; set; } = "speedup";
    public int priority { get; set; } = 0;
    public float speedMultiply = 1f;
    public float duration;
    private Enemy enemy;
    private Coroutine coroutine;
    public void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public float modifyDamage(float damage)
    {
        SpeedUp(speedMultiply, duration);
        return damage;
    }

    IEnumerator SpeedUp(float multiply, float duration)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        else enemy.speed *= multiply;
        yield return new WaitForSeconds(duration);
        if(enemy == null) yield break;
        enemy.speed /= multiply;
        coroutine = null;
    }
}
