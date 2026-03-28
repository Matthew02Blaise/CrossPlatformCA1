using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Speed Variable for the enemy, which can be adjusted in the inspector
    public float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        // Move the enemy left at a constant speed defined in inspector or defaults at 2
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }
}
