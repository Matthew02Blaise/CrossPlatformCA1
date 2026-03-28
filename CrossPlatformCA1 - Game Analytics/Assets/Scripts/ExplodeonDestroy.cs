using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// Spawns an explosion effect + sound when the object is destroyed
public class ExplodeonDestroy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab; // VFX object to spawn
    [SerializeField] private float fxLifetime = 2f;       // how long the VFX stays before auto delete

    [SerializeField] private AudioClip explosionSound;

    private bool suppressed;

    // Used when something removes the object intentionally (like clearing enemies before a boss)
    // prevents explosions triggering during cleanup
    public void Suppress()
    {
        suppressed = true;
    }

    private void OnDestroy()
    {
        // don't explode if suppressed
        if (suppressed) return;
        if (explosionPrefab == null) return;

        // spawn visual effect
        GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // play explosion sound
        SoundManager.Instance.Play(explosionSound, 0.5f);

        // cleanup the VFX after a short time
        Destroy(fx, fxLifetime);
    }
}