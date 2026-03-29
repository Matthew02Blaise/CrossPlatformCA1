using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager Instance;

    private bool isAuthenticated = false;
    private int killCount = 0;

    //achievement
    private bool firstKillUnlocked = false;
    private bool firstDeathUnlocked = false;
    private bool firstBossUnlocked = false;
    private bool secondBossUnlocked = false;
    private bool completedGameUnlocked = false;
    private bool tenKillsUnlocked = false;

    // Ensure that this manager persists across scenes and initializes the Play Games platform
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayGames();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize the Play Games platform and authenticate the user
    private void InitializePlayGames()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success => {
            isAuthenticated = success;
            Debug.Log("Play Games sign in: " + success);
        });
    }

    // This method can be called to report a new score to the leaderboard
    public void ReportScore(long score)
    {
        if (!isAuthenticated) return;

        Social.ReportScore(score, GPGSIds.leaderboard_high_scores, success =>
        {
            Debug.Log("Leaderboard submit: " + success + " | score: " + score);
        });
    }

    // This method can be used to unlock achievements based on specific conditions in the game
    public void UnlockAchievement(string achievementId)
    {
        Debug.Log("UnlockAchievement called. auth=" + isAuthenticated + " id=" + achievementId);
        if (!isAuthenticated) return;

        Social.ReportProgress(achievementId, 100.0f, success =>
        {
            Debug.Log("Achievement unlock: " + achievementId + " | " + success);
        });
    }

    // This method should be called when the player gets a kill
    public void RecordKill()
    {
        killCount++;

        if (!firstKillUnlocked)
        {
            firstKillUnlocked = true;
            UnlockAchievement(GPGSIds.achievement_first_kill);
        }

        if (killCount >= 10 && !tenKillsUnlocked)
        {
            tenKillsUnlocked = true;
            UnlockAchievement(GPGSIds.achievement_10_kills);
        }
    }

    // This method should be called when the player dies for the first time
    public void RecordDeath()
    {
        if (firstDeathUnlocked) return;

        firstDeathUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_first_death);
    }

    // This method should be called when the first boss is defeated
    public void RecordFirstBossDefeated()
    {
        if (firstBossUnlocked) return;

        firstBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_first_boss);
    }

    // This method should be called when the second boss is defeated
    public void RecordSecondBossDefeated()
    {
        if (secondBossUnlocked) return;

        secondBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_second_boss);
    }

    // This method should be called when the game is completed
    public void RecordGameCompleted()
    {
        if (completedGameUnlocked) return;

        completedGameUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_finished_the_game);
    }

    // This method can be called to show the achievements UI
    public void ShowAchievementsUI()
    {
        Debug.Log("ShowAchievementsUI called. isAuthenticated = " + isAuthenticated);
        if (!isAuthenticated) return;
        Social.ShowAchievementsUI();
    }

    // This method can be called to show the leaderboard UI
    public void ShowLeaderboardUI()
    {
        Debug.Log("ShowLeaderboardUI called. isAuthenticated = " + isAuthenticated);
        if (!isAuthenticated) return;
        Social.ShowLeaderboardUI();
    }
}