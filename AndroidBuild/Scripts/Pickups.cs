using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    // list that determines what the pickup actually does
    public enum PickupType
    {
        Shield,
        Health
    }

    [Header("Type")]
    public PickupType type = PickupType.Shield;

    // values applied to the player
    [Header("Values")]
    public int shieldHitsToGive = 1;
    public int healAmount = 1;

    // movement speed
    [Header("Movement")]
    public float speed = 2.5f;

    void Update()
    {
        // constantly moves left like other space objects
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // only react if the player touches it
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        // apply the correct effect
        switch (type)
        {
            case PickupType.Shield:
                player.AddShield(shieldHitsToGive);
                break;

            case PickupType.Health:
                player.Heal(healAmount);
                break;
        }

        // remove pickup after collecting
        Destroy(gameObject);
    }
}