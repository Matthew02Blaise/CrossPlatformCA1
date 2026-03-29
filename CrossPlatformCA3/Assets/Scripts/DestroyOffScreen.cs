using UnityEngine;

// Deletes objects when they go off-screen (when their renderer is no longer visible by any camera).
public class DestroyOffScreen : MonoBehaviour
{
    // Unity calls this automatically when the renderer is no longer visible by any camera
    void OnBecameInvisible()
    {
        // Destroy the entire object - placed this on enemies and bullets to free up memory
        Destroy(gameObject);
    }
}