using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for the eyeball boss's vertical movement, which moves up and down between the top and bottom of the screen at a set speed
public class EyeballBoss : MonoBehaviour
{
    public float speed = 2f;
    int direction = 1;

    Camera cam;
    float minY;
    float maxY;

    // Start is called before the first frame update,
    // which calculates the vertical bounds of the screen based on the camera's size and position
    void Start()
    {
        cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camCenterY = cam.transform.position.y;

        minY = camCenterY - camHeight;
        maxY = camCenterY + camHeight;
    }

    // Update is called once per frame, which moves the boss vertically and changes direction when it hits the bounds of the screen
    void Update()
    {
        transform.Translate(Vector2.up * direction * speed * Time.deltaTime);

        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // Change direction when hitting bounds
        if (transform.position.y <= minY || transform.position.y >= maxY)
        {
            direction *= -1;
        }
    }
}
