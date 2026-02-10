using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs (6 types)")]
    public GameObject[] enemyPrefabs = new GameObject[6];

    [Header("Bosses (Minute 1, 2, 3)")]
    public GameObject[] bossPrefabs = new GameObject[3];

    [Header("Spawn Timing")]
    public float spawnInterval = 1.25f;
    public float startDelay = 1f;

    [Header("Spawn Area")]
    public float spawnX = 10f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Max Alive")]
    public int maxAlive = 8;
    public bool bossesCountTowardsMaxAlive = false;

    private int aliveCount = 0;

    private List<GameObject> activeEnemies = new List<GameObject>();
    public bool IsBossActive { get; private set; }

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (aliveCount >= maxAlive) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);

        GameObject enemy = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        activeEnemies.Add(enemy);
        aliveCount++;
        AttachAliveHook(enemy);
    }

    public void SpawnBoss(int bossIndex)
    {
        // Stop regular enemy spawning
        CancelInvoke(nameof(SpawnEnemy));

        // Despawn all existing enemies
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        activeEnemies.Clear();

        IsBossActive = true;

        if (bossPrefabs == null || bossPrefabs.Length == 0) return;
        if (bossIndex < 0 || bossIndex >= bossPrefabs.Length) return;

        if (bossesCountTowardsMaxAlive && aliveCount >= maxAlive) return;

        GameObject prefab = bossPrefabs[bossIndex];
        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);

        GameObject boss = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        BossDeathHook hook = boss.AddComponent<BossDeathHook>();
        hook.Init(this);

        if (bossesCountTowardsMaxAlive)
        {
            aliveCount++;
            AttachAliveHook(boss);
        }
    }

    private void AttachAliveHook(GameObject obj)
    {
        // Add a tiny helper that notifies this spawner when the enemy is destroyed
        AliveCounterHook hook = obj.AddComponent<AliveCounterHook>();
        hook.Init(this);
    }

    public void NotifyEnemyDestroyed()
    {
        aliveCount = Mathf.Max(0, aliveCount - 1);
    }

    public void NotifyBossDefeated()
    {
        IsBossActive = false;
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }
}

// Helper component: calls back to spawner when this object is destroyed
public class AliveCounterHook : MonoBehaviour
{
    private EnemySpawner spawner;

    public void Init(EnemySpawner s)
    {
        spawner = s;
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.NotifyEnemyDestroyed();
    }
}

public class BossDeathHook : MonoBehaviour
{
    private EnemySpawner spawner;

    public void Init(EnemySpawner s)
    {
        spawner = s;
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.NotifyBossDefeated();
    }
}
