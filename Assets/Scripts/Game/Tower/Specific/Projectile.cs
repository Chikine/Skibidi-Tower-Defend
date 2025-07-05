using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    //public val
    public bool isTrackEnemy = false;
    public float damage;
    public float lifeTime = 5f;
    public float speed;
    //private val
    private Enemy currentEnemy;
    private Transform target;
    private IProjectileAttack bulletAttack;
    private Vector2 direction = Vector2.zero;
    public void Start()
    {
        Destroy(gameObject, lifeTime);
        bulletAttack = GetComponent<IProjectileAttack>();
    }

    public void init(Enemy enemy, float precise)
    {
        currentEnemy = enemy;
        if(isTrackEnemy)
        {
            target = currentEnemy.transform;
            return;
        }
        precise = Mathf.Clamp01(precise);
        direction = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;
        float errorAngle = Random.Range(1f - precise, precise - 1f) * 180;
        direction = Quaternion.Euler(0,0, errorAngle) * direction;
    }

    public void Update()
    {
        if (isTrackEnemy)
        {
            Vector3 enemyDirection = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(enemyDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 180f * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                List<Enemy> enemies = new List<Enemy>();
                enemies.Add(enemy);
                bulletAttack.Attack(enemies, damage);
                Destroy(gameObject);
            }
        }
    }
}
