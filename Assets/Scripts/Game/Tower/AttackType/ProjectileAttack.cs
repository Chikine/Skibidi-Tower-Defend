using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, ITowerAttack
{
    //projectile base stat
    //public float projectileSpeed = 1f;
    public float preciseAim = 1f;
    //amount & reload time
    public int bullets = 1;
    public float reloadInterval = 0;
    public float shootInterval = 0;
    //other
    public GameObject bulletPrefab;
    private Tower tower;

    public void Start()
    {
        tower = GetComponent<Tower>();
        reloadInterval = Mathf.Max(0, reloadInterval);
        shootInterval = Mathf.Max(0, shootInterval);
    }
    public void Attack(List<Enemy> enemiesInRange, float damage)
    {
        StartCoroutine(customAttack(enemiesInRange, damage)); 
    }

    private IEnumerator customAttack(List<Enemy> enemiesInRange, float damage)
    {
        for (int i = 0; i < enemiesInRange.Count && i < bullets; i++)
        {
            if (enemiesInRange[i] != null)
            {
                if (tower.isRotateTower)
                {
                    tower.Rotate(enemiesInRange[i]);
                }
                shoot(enemiesInRange[i]);
                yield return new WaitForSeconds(shootInterval);
            }
        }
    }

    private void shoot(Enemy enemy)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.init(enemy, preciseAim);
        StartCoroutine(bulletReload(reloadInterval));
    }
    private IEnumerator bulletReload(float duration)
    {
        bullets--;
        yield return new WaitForSeconds(duration);
        bullets++;
    }
}
