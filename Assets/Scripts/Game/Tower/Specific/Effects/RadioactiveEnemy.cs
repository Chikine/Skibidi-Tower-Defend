using System.Collections;
using UnityEngine;

public class RadioactiveEnemy : MonoBehaviour, IEffectApply
{
    public string effectName { get; set; } = "RadioactiveEnemy";

    public IEnumerator effectEnumerator(Enemy enemy, string effect, float duration)
    {
        if (enemy.isRadioactive) yield break;
        else
        {
            enemy.isRadioactive = true;
            StartCoroutine(onActiveRadioactive(enemy));
        }
    }

    private IEnumerator onActiveRadioactive(Enemy enemy)
    {
        float currentTime = 1f;
        while (currentTime > 0.2f)
        {
            if (enemy == null) yield break;
            enemy.HP -= 1f;
            enemy.HP *= 0.99f;
            currentTime *= 0.99f;
            yield return new WaitForSeconds(currentTime);
        }
    }
}
