using System.Collections;
using UnityEngine;

public class RevealCamo : MonoBehaviour, IEffectApply
{
    public string effectName { get; set; } = "RevealCamo";
    public bool permanentReveal = false;
    public IEnumerator effectEnumerator(Enemy enemy, string effect, float duration)
    {
        enemy.modifyType("camo", false);
        yield return new WaitForSeconds(duration);
        if(!permanentReveal) enemy.modifyType("camo", true);
    }
}
