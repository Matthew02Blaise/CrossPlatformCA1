using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject shieldPickupPrefab;
    public GameObject healthPickupPrefab;

    // spawn position range
    public float spawnX = 10f;
    public float minY = -4f;
    public float maxY = 4f;

    // random time between spawns
    public float minInterval = 6f;
    public float maxInterval = 12f;

    // probability of spawning a shield instead of health
    [Range(0f, 1f)]
    public float shieldChance = 0.5f;

    void Start()
    {
        // begin the repeating random spawn cycle
        ScheduleNext();
    }

    // schedules the next spawn using a random delay
    void ScheduleNext()
    {
        Invoke(nameof(SpawnPickup), Random.Range(minInterval, maxInterval));
    }

    void SpawnPickup()
    {
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(spawnX, y, 0f);

        // choose which pickup to spawn
        GameObject prefab = (Random.value < shieldChance) ? shieldPickupPrefab : healthPickupPrefab;

        if (prefab != null)
            Instantiate(prefab, pos, prefab.transform.rotation);

        // schedule the following spawn
        ScheduleNext();
    }
}