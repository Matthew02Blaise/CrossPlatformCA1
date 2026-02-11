using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main game manager script that handles the game, including timing, boss spawning, and game end conditions
public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    public float gameDurationSeconds = 180f; // 3 minutes
    private float timeRemaining;

    private bool boss1Spawned, boss2Spawned, boss3Spawned;

    void Start()
    {
        timeRemaining = gameDurationSeconds;
    }

    void Update()
    {
        if (enemySpawner != null && enemySpawner.IsBossActive)
            return;

        timeRemaining -= Time.deltaTime;

        float elapsed = gameDurationSeconds - timeRemaining;

        if (!boss1Spawned && elapsed >= 60f)
        {
            enemySpawner.SpawnBoss(0);
            boss1Spawned = true;
        }

        if (!boss2Spawned && elapsed >= 120f)
        {
            enemySpawner.SpawnBoss(1);
            boss2Spawned = true;
        }

        if (!boss3Spawned && elapsed >= 180f)
        {
            enemySpawner.SpawnBoss(2);
            boss3Spawned = true;
        }

        if (timeRemaining <= 0f && !boss3Spawned)
        {
            EndGame();
        }

        // 
        if (boss3Spawned && enemySpawner != null && !enemySpawner.IsBossActive)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        // TODO: show win/game over UI, stop spawners, etc.
        Time.timeScale = 0f;
    }
}
