using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code for the enemy's shooting behavior, which spawns bullets at a set rate with some random jitter
public class EnemyShoot : MonoBehaviour
{
    // Prefab for the enemy's bullet and the point from which the bullet will be fired, both set in the inspector
    public GameObject enemyBulletPrefab;
    public Transform firePoint;

    // Fire rate variable for the enemy, which can be adjusted in the inspector
    public float fireRate = 1f;


    private float nextShotTime;

    // Start is called before the first frame update
    void Start()
    {
        //calls bullet interval to set the first shot time based on the fire rate
        BulletInterval();
    }

    void Update()
    {
        // Check if it's time to shoot based on the fire rate
        if (Time.time >= nextShotTime)
        {
            Shoot();
            BulletInterval();
        }
    }

    // Method to instantiate the enemy bullet at the fire point's position
    void Shoot()
    {
        // Check if the bullet prefab and fire point are assigned before trying to instantiate
        if (enemyBulletPrefab == null || firePoint == null) return;
        Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
    }

    // Method to calculate the next shot time based on the fire rate, with a safeguard against zero or negative fire rates
    void BulletInterval()
    {
        // If fireRate is zero or negative, set a very long delay to effectively prevent shooting
        float delay = fireRate <= 0f ? 9999f : 1f / fireRate;
        nextShotTime = Time.time + delay;
    }
}
