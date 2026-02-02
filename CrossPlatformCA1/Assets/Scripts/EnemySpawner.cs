using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs = new GameObject[6];

    public float spawnInterval = 1.25f;
    public float startDelay = 1f;

    public float spawnX = 10f;
    public float minY = -4f;
    public float maxY = 4f;

    public int[] weights = new int[6] { 30, 25, 20, 15, 7, 3 };

    public int health = 1;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        int index = GetWeightedRandomIndex();
        if (enemyPrefabs[index] == null)
            return;

        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, y, 0f);

        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }

    int GetWeightedRandomIndex()
    {
        if (weights == null || weights.Length != enemyPrefabs.Length)
            return Random.Range(0, enemyPrefabs.Length);

        int total = 0;
        for (int i = 0; i < weights.Length; i++)
            total += Mathf.Max(0, weights[i]);

        if (total <= 0)
            return Random.Range(0, enemyPrefabs.Length);

        int roll = Random.Range(0, total);
        int running = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            running += Mathf.Max(0, weights[i]);
            if (roll < running)
                return i;
        }

        return enemyPrefabs.Length - 1;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
