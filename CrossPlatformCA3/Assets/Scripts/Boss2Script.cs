using System.Collections;
using UnityEngine;

public class Boss2Script : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;

    private int direction = 1; 
    private Camera cam;
    private float minY;
    private float maxY;

    [Header("Spawn minions")]
    public GameObject smallEnemyPrefab;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;
    public float spawnRadiusY = 2.5f;   // random Y offset around boss
    public float spawnOffsetX = -1.5f;  // spawn slightly left of boss

    [Header("Acid Spit")]
    public GameObject acidProjectilePrefab;
    public Transform acidMouthPoint;
    public float minAcidInterval = 1.2f;
    public float maxAcidInterval = 2.5f;
    public float acidSpeed = 8f;
    public bool aimAtPlayer = true;

    private Transform player; // grabs player position for aiming acid

    private void Start()
    {
        // Cache the main camera, can clamp boss movement to screen vertically
        cam = Camera.main;

        // If camera is missing for some reason, dont clamp
        if (cam != null)
        {
            float camHeight = cam.orthographicSize;           // half-height of the camera in world units
            float camCenterY = cam.transform.position.y;

            minY = camCenterY - camHeight;
            maxY = camCenterY + camHeight;
        }
        else
        {
            // no clamp
            minY = float.NegativeInfinity;
            maxY = float.PositiveInfinity;
        }

        // Find the player
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null) player = ph.transform;

        // Start the boss's attack patterns
        StartCoroutine(SpawnSmallEnemiesLoop());
        StartCoroutine(AcidSpitLoop());
    }

    private void Update()
    {
        // Move boss vertically every frame
        transform.Translate(Vector2.up * direction * speed * Time.deltaTime);

        // Clamp Y so it stays within the visible camera range
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // bounces boss from top to bottom of screen
        if (transform.position.y <= minY || transform.position.y >= maxY)
            direction *= -1;
    }

    // Loop: spawn minions for duration of fight
    private IEnumerator SpawnSmallEnemiesLoop()
    {
        while (true)
        {
            // random delay between spawns
            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);

            // if prefab isn't set in the inspector then skip
            if (smallEnemyPrefab == null) continue;

            // spawn around boss with a random vertical offset
            float y = transform.position.y + Random.Range(-spawnRadiusY, spawnRadiusY);
            Vector3 spawnPos = new Vector3(transform.position.x + spawnOffsetX, y, 0f);

            // spawn
            Instantiate(smallEnemyPrefab, spawnPos, Quaternion.identity);
        }
    }


    // Loop: spit acid for duration of fight
    private IEnumerator AcidSpitLoop()
    {
        while (true)
        {
            // random delay between spits
            float wait = Random.Range(minAcidInterval, maxAcidInterval);
            yield return new WaitForSeconds(wait);

            if (acidProjectilePrefab == null) continue;

            // spawn from mouth point, otherwise from boss position
            Vector3 spawnPos = (acidMouthPoint != null) ? acidMouthPoint.position : transform.position;

            // create the projectile
            GameObject acid = Instantiate(acidProjectilePrefab, spawnPos, Quaternion.identity);

            //play sound effect on spit
            SoundManager.Instance.Play(SoundManager.Instance.boss2AcidSpit);

            // add velocity to projectile
            Rigidbody2D rb = acid.GetComponent<Rigidbody2D>();

            // Default spit direction is left (classic shmup behaviour)
            Vector2 dir = Vector2.left;

            // If enabled, aim at player
            if (aimAtPlayer && player != null)
                dir = ((Vector2)(player.position - spawnPos)).normalized;

            rb.velocity = dir * acidSpeed;
        }
    }
}