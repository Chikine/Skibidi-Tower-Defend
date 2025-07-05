using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAttack : MonoBehaviour, ITowerAttack, IProjectileAttack
{
    private Tower tower;
    //addition: multiple attack
    public float applyAfterSeconds;
    //duration
    public float duration;
    //attack all ?
    public bool attackAll = false;
    //other
    public bool showRangeWhenAttack;
    private List<IEffectApply> effectApplyList = new();
    public void Start()
    {
        tower = GetComponent<Tower>();
        effectApplyList.AddRange(GetComponents<IEffectApply>());
    }

    public void Attack(List<Enemy> enemiesInRange, float damage)
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                EffectManager.instance.startEffectCoroutine(enemy, effectApplyList, applyAfterSeconds, duration);
                StartCoroutine(playAnim());
                if (showRangeWhenAttack)
                {
                    AppearRangeWhenPlayAnimation appearRangeWhenPlayAnimation = GetComponentInChildren<AppearRangeWhenPlayAnimation>();
                    if (appearRangeWhenPlayAnimation != null)
                    {
                        appearRangeWhenPlayAnimation.showRange();
                    }
                    else Debug.Log("range is null");
                }
            }
            if(!attackAll) break;
        }
    }

    IEnumerator playAnim()
    {
        yield return new WaitForSeconds(applyAfterSeconds);
        if (tower != null) tower.PlayAnim();
    }
}
