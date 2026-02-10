using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballBoss : MonoBehaviour
{
    public float speed = 2f;
    int direction = 1;

    Camera cam;
    float minY;
    float maxY;

    void Start()
    {
        cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camCenterY = cam.transform.position.y;

        minY = camCenterY - camHeight;
        maxY = camCenterY + camHeight;
    }

    void Update()
    {

        transform.Translate(Vector2.up * direction * speed * Time.deltaTime);

        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // Reverse direction when hitting bounds
        if (transform.position.y <= minY || transform.position.y >= maxY)
        {
            direction *= -1;
        }
    }
}
