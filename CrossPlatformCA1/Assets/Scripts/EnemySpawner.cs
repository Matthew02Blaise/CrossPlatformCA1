using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main spawner script that handles spawning regular enemies and bosses,
// with options for spawn intervals, max alive enemies, and boss behavior
public class EnemySpawner : MonoBehaviour
{
    //Varivales for enemy and boss prefabs, spawn intervals, spawn area and max alive enemies, all can be changed in the inspector
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs = new GameObject[6];

    [Header("Boss Prefabs")]
    public GameObject[] bossPrefabs = new GameObject[3];

    [Header("Spawn interval")]
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

    // Keep track of active enemies to despawn them when a boss spawns
    private List<GameObject> activeEnemies = new List<GameObject>();
    // Flag to indicate if a boss is currently active, which can be used by other scripts to adjust behavior
    public bool IsBossActive { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Start the regular enemy spawning loop
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    // Method to spawn a regular enemy, which checks the max alive limit and randomly selects a prefab and spawn position
    void SpawnEnemy()
    {
        // Don't spawn regular enemies if a boss is active or if we've reached the max alive limit
        if (aliveCount >= maxAlive) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // Randomly select an enemy prefab from the array
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        if (prefab == null) return;

        // Randomly determine a spawn position within the defined Y range, using the fixed X spawn position
        Vector3 spawnPos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);

        // Instantiate the enemy prefab at the spawn position with its default rotation
        GameObject enemy = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        // Add the new enemy to the list of active enemies and increment the alive count,
        // then attach a hook to track when it gets destroyed
        activeEnemies.Add(enemy);
        aliveCount++;
        AttachAliveHook(enemy);
    }

    // Method to spawn a boss, which stops regular enemy spawning, despawns existing enemies, and instantiates the boss prefab
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
        // Clear the list of active enemies and reset the alive count since they are all destroyed
        activeEnemies.Clear();
        // Set the flag to indicate a boss is now active, which can be used by other scripts to adjust behavior
        IsBossActive = true;

        // Validate the boss index and prefab array before attempting to spawn the boss
        if (bossPrefabs == null || bossPrefabs.Length == 0) return;
        if (bossIndex < 0 || bossIndex >= bossPrefabs.Length) return;

        // If bosses count towards max alive, check the limit before spawning the boss
        if (bossesCountTowardsMaxAlive && aliveCount >= maxAlive) return;

        // Randomly determine a spawn position for the boss within the defined Y range, using the fixed X spawn position
        GameObject prefab = bossPrefabs[bossIndex];
        if (prefab == null) return;

        // Instantiate the boss prefab at the spawn position with its default rotation
        Vector3 spawnPos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);

        // Instantiate the boss and attach a hook to track when it gets defeated,
        // which will allow us to resume regular enemy spawning
        GameObject boss = Instantiate(prefab, spawnPos, prefab.transform.rotation);
        BossDeathHook hook = boss.AddComponent<BossDeathHook>();
        hook.Init(this);

        // If bosses count towards max alive, increment the alive count and attach a hook to track when the boss is defeated
        if (bossesCountTowardsMaxAlive)
        {
            aliveCount++;
            AttachAliveHook(boss);
        }
    }

    // Helper method to attach a hook component to an enemy or boss GameObject,
    // which will notify this spawner when it gets destroyed
    private void AttachAliveHook(GameObject obj)
    {
        // Add a tiny helper that notifies this spawner when the enemy is destroyed
        AliveCounterHook hook = obj.AddComponent<AliveCounterHook>();
        hook.Init(this);
    }

    // Method called by the AliveCounterHook when an enemy is destroyed,
    // which decrements the alive count and ensures it doesn't go below zero
    public void NotifyEnemyDestroyed()
    {
        aliveCount = Mathf.Max(0, aliveCount - 1);
    }

    // Method called by the BossDeathHook when a boss is defeated,
    // which resets the boss active flag and resumes regular enemy spawning
    public void NotifyBossDefeated()
    {
        IsBossActive = false;
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }
}

// Helper component that notifies the EnemySpawner when an enemy is destroyed, allowing it to keep track of the alive count
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

// Helper component that notifies the EnemySpawner when a boss is defeated, allowing it to resume regular enemy spawning
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
