using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyHealth : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;

    [Header("Health")]
    public int health = 1;

    void Update()
    {

    }

    // Player body collision
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(1);
            //Destroy(gameObject);
        }
    }

    //
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
