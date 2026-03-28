using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    //spawns a bullet at the fire point's position when called, and plays the shooting sound effect
    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        SoundManager.Instance.Play(SoundManager.Instance.playerShoot, 0.2f);
    }
}
