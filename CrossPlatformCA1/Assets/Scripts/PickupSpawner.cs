using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject shieldPickupPrefab;
    public GameObject healthPickupPrefab;

    public float spawnX = 10f;
    public float minY = -4f;
    public float maxY = 4f;

    public float minInterval = 6f;
    public float maxInterval = 12f;

    [Range(0f, 1f)]
    public float shieldChance = 0.5f;

    void Start()
    {
        ScheduleNext();
    }

    void ScheduleNext()
    {
        Invoke(nameof(SpawnPickup), Random.Range(minInterval, maxInterval));
    }

    void SpawnPickup()
    {
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(spawnX, y, 0f);

        GameObject prefab =
            (Random.value < shieldChance) ? shieldPickupPrefab : healthPickupPrefab;

        if (prefab != null)
            Instantiate(prefab, pos, prefab.transform.rotation);

        ScheduleNext();
    }
}
