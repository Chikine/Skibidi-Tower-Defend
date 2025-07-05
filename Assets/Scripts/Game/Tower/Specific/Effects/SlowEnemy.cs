using System.Collections;
using UnityEngine;

public class SlowEnemy : MonoBehaviour, IEffectApply
{
    public string effectName { get; set; } = "SlowEnemy";
    public float slowMultiple;
    private Enemy enemy;
    private Main main;
    private float OriginSpeed = 1f;
    public void Start()
    {
        enemy = GetComponent<Enemy>();
        if(enemy != null)
        {
            OriginSpeed = enemy.speed;
        }
        main = FindFirstObjectByType<Main>();
        slowMultiple /= main.gameData.debuffEfficiency;
    }
    public IEnumerator effectEnumerator(Enemy enemy, string effect, float duration)
    {
        enemy.isSlow = true;
        enemy.SetSpeed(OriginSpeed * slowMultiple);
        yield return new WaitForSeconds(duration);
        enemy.isSlow = false;
        enemy.SetSpeed(OriginSpeed);
        enemy.RemoveEffectKey(effect);
    }
}
