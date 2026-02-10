using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public Transform firePoint;

    public float fireRate = 1f;
    public float randomJitter = 0.2f;

    private float nextShotTime;

    void Start()
    {
        BulletInterval();
    }

    void Update()
    {
        if (Time.time >= nextShotTime)
        {
            Shoot();
            BulletInterval();
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab == null || firePoint == null) return;
        Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
    }

    void BulletInterval()
    {
        float delay = fireRate <= 0f ? 9999f : 1f / fireRate;
        nextShotTime = Time.time + delay + Random.Range(0f, randomJitter);
    }
}
