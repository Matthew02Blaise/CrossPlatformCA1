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

    public int health = 1;
    public int maxAlive = 8;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        int index = GetRandomIndex();
        if (enemyPrefabs[index] == null)
            return;

        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, y, 0f);

        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }

    int GetRandomIndex()
    {
        return Random.Range(0, enemyPrefabs.Length);
    }
}
