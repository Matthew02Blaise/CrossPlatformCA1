using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyHealth : MonoBehaviour
{
    // Health variable for the enemy, which can be adjusted in the inspector
    public int health = 1;

    // Check for collision with the player and damage the player on contact
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has a PlayerHealth component
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        // If the player component exists, damage the player
        if (player != null)
        {
            player.TakeDamage(1);
        }
    }

    // Method to take damage and check if the enemy should be destroyed
    public void TakeDamage(int damage)
    {
        // Reduce health by the damage amount
        health -= damage;

        // If health is zero or less, destroy the enemy
        if (health <= 0)
        {
            Destroy(gameObject);
            //play sound on enemy death
            SoundManager.Instance.Play(SoundManager.Instance.enemyDeath);
        }
    }
}
