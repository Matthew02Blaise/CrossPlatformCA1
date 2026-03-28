using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles spawning normal enemies, bosses, stage progression and score hooks
public class EnemySpawner : MonoBehaviour
{
    // Enemy and boss prefabs assigned in inspector
    public GameObject[] enemyPrefabs = new GameObject[6];
    public GameObject[] bossPrefabs = new GameObject[3];

    // Timing
    public float spawnInterval = 1.25f;
    public float startDelay = 1f;

    // Spawn positions
    public float spawnX = 25f;
    public float bossSpawnX = 5f;
    public float minY = -12f;
    public float maxY = 12f;

    // Max enemies alive at once
    public int maxAlive = 8;
    public bool bossesCountTowardsMaxAlive = false;

    // Score values per enemy/boss
    public int[] enemyScores = new int[6] { 1, 3, 1, 2, 4, 2 };
    public int[] bossScores = new int[3] { 10, 20, 50 };

    private GameManager gameManager;
    private int aliveCount = 0;

    // Track active enemies so we can wipe them when a boss spawns
    private List<GameObject> activeEnemies = new List<GameObject>();

    // Lets other scripts know a boss fight is happening
    public bool IsBossActive { get; private set; }

    // Enemy pools change after each boss
    public GameObject[] stage1Enemies;
    public GameObject[] stage2Enemies;
    public GameObject[] stage3Enemies;
    public GameObject[] stage4Enemies;

    private GameObject[] currentEnemies;
    private int currentBossIndex = -1;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        currentEnemies = stage1Enemies;

        // continuously spawns enemies
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        // don't spawn if limit reached
        if (aliveCount >= maxAlive) return;
        if (currentEnemies == null || currentEnemies.Length == 0) return;

        GameObject prefab = currentEnemies[Random.Range(0, currentEnemies.Length)];
        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);
        GameObject enemy = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        // attach score handler so killing enemy adds score
        int points = GetEnemyPoints(prefab);
        var scoreHook = enemy.AddComponent<scoreUpdate>();
        scoreHook.Init(gameManager, points);

        activeEnemies.Add(enemy);
        aliveCount++;
        AttachAliveHook(enemy);
    }

    public void SpawnBoss(int bossIndex)
    {
        MusicManager.Instance.PlayBoss(bossIndex);
        currentBossIndex = bossIndex;

        // stop normal spawning during boss fight
        CancelInvoke(nameof(SpawnEnemy));

        // remove all normal enemies before boss
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                var hook = enemy.GetComponent<scoreUpdate>();
                if (hook != null) hook.Suppress();
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();
        IsBossActive = true;

        if (bossPrefabs == null || bossIndex < 0 || bossIndex >= bossPrefabs.Length) return;

        GameObject prefab = bossPrefabs[bossIndex];
        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(bossSpawnX, Random.Range(minY, maxY), 0f);
        GameObject boss = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        // score for killing boss
        int bossPoints = (bossScores != null && bossIndex < bossScores.Length) ? bossScores[bossIndex] : 0;
        var scoreHook = boss.AddComponent<scoreUpdate>();
        scoreHook.Init(gameManager, bossPoints);

        // notify spawner when boss dies
        BossDeathHook hook2 = boss.AddComponent<BossDeathHook>();
        hook2.Init(this);

        if (bossesCountTowardsMaxAlive)
        {
            aliveCount++;
            AttachAliveHook(boss);
        }
    }

    private void AttachAliveHook(GameObject obj)
    {
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

        // change enemy pool after each boss
        if (currentBossIndex == 0) currentEnemies = stage2Enemies;
        else if (currentBossIndex == 1) currentEnemies = stage3Enemies;
        else if (currentBossIndex == 2) currentEnemies = stage4Enemies;

        // resume normal spawning
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);

        MusicManager.Instance.PlayNormal();
    }

    int GetEnemyPoints(GameObject prefab)
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i] == prefab)
                return (enemyScores != null && i < enemyScores.Length) ? enemyScores[i] : 0;
        }
        return 0;
    }
}

// attached to every spawned enemy so the spawner knows when it dies
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

// attached to bosses so the spawner can resume waves after boss death
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