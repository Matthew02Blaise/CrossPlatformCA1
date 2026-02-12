using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int health = 3;

    [Header("Shield")]
    public int shieldHits = 0; // blocks this many hits

    private bool isDead;

    public GameObject shieldText;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Shield blocks first
        if (shieldHits > 0)
        {
            shieldHits--;
            return;
        }

        health -= damage;
        if (health <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        health = Mathf.Min(maxHealth, health + amount);
    }

    public void AddShield(int hits)
    {
        if (isDead) return;
        shieldHits += hits;


        if (shieldText != null)
            shieldText.SetActive(true);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        Destroy(gameObject);
    }
}
