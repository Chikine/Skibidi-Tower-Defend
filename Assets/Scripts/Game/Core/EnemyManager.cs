using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public float durationBetweenRound = 1f;
    private List<Enemy> enemyOnScreen = new();
    private Coroutine nextRound;
    private Rounds rounds;

    public void Start()
    {
        rounds = FindFirstObjectByType<Rounds>();
    }
    public void Register(Enemy enemy)
    {
        enemyOnScreen.Add(enemy);
    }

    public void Unregister(Enemy enemy)
    {
        enemyOnScreen.Remove(enemy);
        nextRound = StartCoroutine(checkNextRound());
    }

    private IEnumerator checkNextRound()
    {
        if (enemyOnScreen.Count > 0) yield break;
        if (!rounds.isAllCoroutinesFinish()) yield break;
        yield return new WaitForSeconds(durationBetweenRound);
        rounds.getNextRound();
        if (nextRound != null)
        {
            StopCoroutine(nextRound);
        }
        nextRound = null;
    }
}
