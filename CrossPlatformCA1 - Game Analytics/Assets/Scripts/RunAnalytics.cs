using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnalytics : MonoBehaviour
{
    public static RunAnalytics Instance { get; private set; }

    bool runActive;

    float runStartTime;
    float bossStartTime;
    bool bossActive;
    int currentBossIndex = -1;

    float totalPausedSeconds;        // time spent in boss fights (your timer pauses)
    int totalDamageTaken;            // health damage only
    int totalShieldHitsBlocked;      // count of hits blocked by shield (or you can track damage)

    int lastHealth;
    int lastShield;

    int shotsFired;
    int shotsHit;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Call once when gameplay begins
    public void StartRun()
    {
        if (runActive) return;

        runActive = true;
        runStartTime = Time.time;

        // Progression event: treat whole attempt as a "Run"
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Run");
    }

    public void OnBossSpawn(int bossIndex, int scoreAtSpawn)
    {
        if (!runActive) return;

        currentBossIndex = bossIndex;
        bossActive = true;
        bossStartTime = Time.time;

        GameAnalytics.NewDesignEvent($"boss:spawn:{bossIndex + 1}");
        GameAnalytics.NewDesignEvent($"boss:score_at_spawn:{bossIndex + 1}", scoreAtSpawn);
    }

    public void OnBossDefeated(int bossIndex, int scoreAtDefeat)
    {
        if (!runActive) return;
        if (!bossActive) return;

        bossActive = false;

        float bossDuration = Time.time - bossStartTime;
        totalPausedSeconds += bossDuration;

        GameAnalytics.NewDesignEvent($"boss:defeat:{bossIndex + 1}", bossDuration);
        GameAnalytics.NewDesignEvent($"boss:score_at_defeat:{bossIndex + 1}", scoreAtDefeat);
    }

    public void RecordShieldBlockedHit(int hits = 1)
    {
        if (!runActive) return;
        totalShieldHitsBlocked += hits;
    }

    public void RecordHealthDamage(int damage)
    {
        if (!runActive) return;
        totalDamageTaken += Mathf.Max(0, damage);
    }

    public void UpdateVitals(int health, int shield)
    {
        lastHealth = health;
        lastShield = shield;
    }

    public float GetRunElapsedSeconds()
    {
        if (!runActive && runStartTime <= 0f) return 0f;
        return Time.time - runStartTime;
    }

    public void RecordShotFired(int count = 1)
    {
        if (!runActive) return;
        shotsFired += Mathf.Max(0, count);
    }

    public void RecordShotHit(int count = 1)
    {
        if (!runActive) return;
        shotsHit += Mathf.Max(0, count);
    }

    // Call once on win/lose
    public void EndRun(bool win, int finalScore)
    {
        if (!runActive) return;
        runActive = false;

        // Summary (keep these low-frequency)
        GameAnalytics.NewDesignEvent("run:damage_taken", totalDamageTaken);
        GameAnalytics.NewDesignEvent("run:shield_hits_blocked", totalShieldHitsBlocked);
        GameAnalytics.NewDesignEvent("run:boss_pause_time", totalPausedSeconds);

        // Useful context
        GameAnalytics.NewDesignEvent("run:end_health", lastHealth);
        GameAnalytics.NewDesignEvent("run:end_shield", lastShield);
        float accuracy = (shotsFired > 0) ? (float)shotsHit / shotsFired : 0f;
        GameAnalytics.NewDesignEvent("run:accuracy", accuracy);

        if (win)
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Run", finalScore);
        else
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Run", finalScore);

        // reset counters for next attempt
        totalPausedSeconds = 0;
        totalDamageTaken = 0;
        totalShieldHitsBlocked = 0;
        bossActive = false;
        currentBossIndex = -1;
        shotsFired = 0;
        shotsHit = 0;
        runStartTime = 0f;
    }
}
