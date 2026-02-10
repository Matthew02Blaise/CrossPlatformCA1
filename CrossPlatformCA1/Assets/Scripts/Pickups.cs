using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        Shield,
        Health
    }

    [Header("Type")]
    public PickupType type = PickupType.Shield;

    [Header("Values")]
    public int shieldHitsToGive = 1;
    public int healAmount = 1;

    [Header("Movement")]
    public float speed = 2.5f;

    void Update()
    {
        // Move left constantly
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        // Apply effect based on type
        switch (type)
        {
            case PickupType.Shield:
                player.AddShield(shieldHitsToGive);
                break;

            case PickupType.Health:
                player.Heal(healAmount);
                break;
        }

        Destroy(gameObject);
    }
}
