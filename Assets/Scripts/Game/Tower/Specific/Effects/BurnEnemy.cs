using System.Collections;
using UnityEngine;

public class BurnEnemy : MonoBehaviour, IEffectApply
{
    public string effectName { get; set; } = "BurnEnemy";
    public float burnInterval;
    public float burnDamage;

    public bool stackableBurn;
    public bool eternalBurn;

    private Main main;

    private void Start()
    {
        main = FindFirstObjectByType<Main>();
        burnDamage *= main.gameData.debuffEfficiency;
        burnInterval *= main.gameData.debuffEfficiency;
    }
    public IEnumerator effectEnumerator(Enemy enemy, string effect, float duration)
    {
        if (eternalBurn)
        {
            if (enemy.isBurn) { 
                yield break;
            }
            StartCoroutine(DOT(enemy, duration, 0, burnDamage * (enemy.isType("freezer") ? 1.5f : 1f)));
            enemy.isBurn = true;
            yield break;
        }
        enemy.isBurn = true;
        yield return StartCoroutine(DOT(enemy, duration, burnInterval, burnDamage * (enemy.isType("freezer") ? 1.5f : 1f)));
        enemy.isBurn = false;
        enemy.RemoveEffectKey(effect);
    }
    IEnumerator DOT(Enemy enemy, float duration, float interval, float damage)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += interval;
            if (enemy != null) enemy.TakeDamage(damage);
            else yield break;
            yield return new WaitForSeconds(duration);
        }
    }
}
