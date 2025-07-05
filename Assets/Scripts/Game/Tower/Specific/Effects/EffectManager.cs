using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    private Main main;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        main = FindFirstObjectByType<Main>();
    }
    public Coroutine startEffectCoroutine(Enemy enemy, List<IEffectApply> effectApplyList, float applyAfterSeconds, float duration)
    {
        return StartCoroutine(ExecuteApply(enemy, effectApplyList, applyAfterSeconds, duration));
    }
    private IEnumerator ExecuteApply(Enemy enemy, List<IEffectApply> effectApplyList, float applyAfterSeconds, float duration)
    {
        yield return new WaitForSeconds(applyAfterSeconds);
        if (enemy.isTypeActivate("shield")) yield break;
        //reduce duration base on efficiency
        duration *= main.gameData.debuffEfficiency;
        //apply each avail effect 
        foreach (IEffectApply effectApply in effectApplyList)
        {
            enemy.ApplyEffect(effectApply.effectName, effectApply.effectEnumerator, duration);
        }
    }
}
