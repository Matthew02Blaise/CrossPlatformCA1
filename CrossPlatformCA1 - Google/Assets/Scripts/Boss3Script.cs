using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Script : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject minePrefab;       // mine
    public GameObject asteroidPrefab;   // asteroid

    public float minSpawnInterval = 1.5f;   // fastest spawn time
    public float maxSpawnInterval = 3.5f;   // slowest spawn time

    public float spawnOffsetX = -1.5f;  // spawn slightly to the left of boss
    public float spawnSpreadY = 6f;     // random vertical range around boss

    [Range(0f, 1f)]
    public float mineChance = 0.5f;     // probability of spawning a mine instead of asteroid (50%)

    [Header("Laser")]
    public LaserBeam laserPrefab;
    public Transform laserOrigin;       // shoot point
    public float minLaserInterval = 5f; // fast spawn time
    public float maxLaserInterval = 9f; // slow spawn time
    public float laserWarmup = 0.6f;    // delay before firing so player can react

    private Transform player;           // stores player position for laser targeting

    void Start()
    {
        // Find player once at start instead of every frame for laser
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null) player = ph.transform;

        // run both spawn and laser loops concurrently
        StartCoroutine(SpawnLoop());
        StartCoroutine(LaserLoop());
    }

    // spawns mines or asteroids at random intervals
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // wait random time between spawns
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            // decide which object to spawn
            bool spawnMine = Random.value < mineChance;
            GameObject prefab = spawnMine ? minePrefab : asteroidPrefab;

            // check in case prefab missing in inspector
            if (prefab == null) continue;

            // pick random vertical position around boss
            float y = transform.position.y + Random.Range(-spawnSpreadY, spawnSpreadY);

            // spawn slightly in front of boss
            Vector3 pos = new Vector3(transform.position.x + spawnOffsetX, y, 0f);

            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    // Fires a tracking laser that locks onto the player's position
    IEnumerator LaserLoop()
    {
        while (true)
        {
            // wait random delay between laser attacks
            yield return new WaitForSeconds(Random.Range(minLaserInterval, maxLaserInterval));

            if (laserPrefab == null) continue;

            // get the player's current position for targeting, or default to a point left
            Vector3 targetPos = (player != null)
                ? player.position
                : (transform.position + Vector3.left * 10f);

            // warmup delay before firing so player has a chance to dodge
            if (laserWarmup > 0f)
                yield return new WaitForSeconds(laserWarmup);

            // shoot from assignable origin point, or default to boss position if errors 
            Vector3 originPos = (laserOrigin != null) ? laserOrigin.position : transform.position;

            // create the beam and tell it where to fire
            LaserBeam beam = Instantiate(laserPrefab, originPos, Quaternion.identity);
            beam.Init(originPos, targetPos);

            // play boss attack sound
            SoundManager.Instance.Play(SoundManager.Instance.boss3LaserFire);
        }
    }
}