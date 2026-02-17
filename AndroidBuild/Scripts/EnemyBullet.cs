using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code for the enemy's bullet, which moves left and damages the player on contact
public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;

    // Update is called once per frame
    void Update()
    {
        // Move the bullet left
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hits the player
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        // If it does, damage the player and destroy the bullet
        if (player != null)
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
