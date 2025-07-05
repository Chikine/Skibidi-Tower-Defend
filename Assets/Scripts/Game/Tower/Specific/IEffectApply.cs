using System.Collections;
using UnityEngine;

public interface IEffectApply
{
    string effectName { get; set; }
    IEnumerator effectEnumerator(Enemy enemy, string effect, float duration);
}
