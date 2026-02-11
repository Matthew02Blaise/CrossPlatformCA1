using UnityEngine;

//Code used on enemies and bullets to destroy them when they go off screen
public class DestroyOffScreen : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
