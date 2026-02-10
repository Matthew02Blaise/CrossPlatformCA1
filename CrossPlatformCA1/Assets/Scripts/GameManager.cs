using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
        timeRemaining -= Time.deltaTime;

        float elapsed = gameDurationSeconds - timeRemaining;

        // Boss each minute (60, 120, 180 elapsed)
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

            // End the game here (win condition). Replace with your own UI.
            EndGame();
        }

        if (timeRemaining <= 0f && !boss3Spawned)
        {
            // Safety fallback
            EndGame();
        }
    }

    void EndGame()
    {
        // TODO: show win/game over UI, stop spawners, etc.
        Time.timeScale = 0f;
    }
}
