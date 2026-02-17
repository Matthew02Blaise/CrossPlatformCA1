using UnityEngine;

// acid projectile  behaviour: - Self-destructs if it enters a trigger with a PlayerHealth component, it deals damage once and then destroys itself.
public class AcidScript : MonoBehaviour
{
    [Header("Damage")]
    [Tooltip("How much damage this acid deals on hit.")]
    [SerializeField] private int damage = 1;

    [Header("Lifetime")]
    [Tooltip("Seconds before this acid object auto-destroys (cleanup / performance).")]
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        // Auto-cleanup after lifeTime seconds, even if nothing is hit.
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // find PlayerHealth component
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        // If there was no PlayerHealth, do nothing
        if (player == null) return;

        // Deal damage once.
        player.TakeDamage(damage);

        // Destroy the acid so it doesn't keep damaging on subsequent trigger events.
        Destroy(gameObject);
    }
}