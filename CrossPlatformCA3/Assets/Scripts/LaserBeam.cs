using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float length = 40f;
    public float width = 1.2f;
    public float duration = 1.2f;
    public int damage = 1;
    public float damageTick = 0.25f;

    private Vector2 dir;
    private float nextDamage;

    public void Init(Vector3 origin, Vector3 target)
    {
        dir = (target - origin).normalized;

        // rotate toward locked position
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // position in the middle of the beam
        transform.position = origin + (Vector3)(dir * length * 0.5f);

        // scale the rectangle to beam size
        transform.localScale = new Vector3(length, width, 1f);

        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < nextDamage) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
            nextDamage = Time.time + damageTick;
        }
    }
}
