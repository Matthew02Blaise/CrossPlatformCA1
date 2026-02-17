using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// Main controller for game
// Handles timer, boss progression, score and ending the game.
public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    public float gameDurationSeconds = 180f; // total run time (3 minutes + time player spends on bosses fluxuates)
    private float timeRemaining;

    private int bossStage = 0;

    private HUD hud;
    private int lastShownSecond = -1;

    public int Score { get; private set; }

    [SerializeField] private EndUI EndUI;

    // sets up timer and HUD at start, also starts music
    void Start()
    {
        timeRemaining = gameDurationSeconds;

        hud = FindFirstObjectByType<HUD>();

        UpdateTimerHUD(force: true);
        if (hud != null) hud.SetScore(Score);

        MusicManager.Instance.PlayNormal();
    }

    void Update()
    {
        //onlyy allows testing shortcuts in the editor
        #if UNITY_EDITOR
                // quick shortcuts for testing boss phases
                if (Input.GetKeyDown(KeyCode.Alpha1)) timeRemaining = gameDurationSeconds - 60f;
                if (Input.GetKeyDown(KeyCode.Alpha2)) timeRemaining = gameDurationSeconds - 120f;
                if (Input.GetKeyDown(KeyCode.Alpha3)) timeRemaining = gameDurationSeconds - 179f;
        #endif

        // pause the timer while a boss fight is active
        if (enemySpawner != null && enemySpawner.IsBossActive)
            return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerHUD();

        float elapsed = gameDurationSeconds - timeRemaining;

        // boss checkpoints
        if (bossStage == 0 && elapsed >= 60f)
        {
            enemySpawner.SpawnBoss(0);
            bossStage = 1;
            return;
        }

        if (bossStage == 1 && elapsed >= 120f)
        {
            enemySpawner.SpawnBoss(1);
            bossStage = 2;
            return;
        }

        if (bossStage == 2 && elapsed >= 180f)
        {
            enemySpawner.SpawnBoss(2);
            bossStage = 3;
            return;
        }

        // after final boss dies → win
        if (bossStage == 3 && enemySpawner != null && !enemySpawner.IsBossActive)
        {
            bossStage = 4;
            EndGame();
        }
    }

    // updates HUD but only when the visible second changes
    private void UpdateTimerHUD(bool force = false)
    {
        if (hud == null) return;

        int currentSecond = Mathf.Max(0, Mathf.CeilToInt(timeRemaining));
        if (!force && currentSecond == lastShownSecond) return;

        lastShownSecond = currentSecond;
    }

    // called by enemies/bosses when destroyed
    public void AddScore(int amount)
    {
        Score += amount;
        if (hud != null) hud.SetScore(Score);
    }

    // shows win screen and stops gameplay
    void EndGame()
    {
        EndUI.ShowWin();

        Time.timeScale = 0f;
    }
}