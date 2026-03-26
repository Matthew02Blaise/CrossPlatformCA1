using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager Instance;

    private bool isAuthenticated = false;
    private int killCount = 0;

    //achievement booleans
    private bool firstKillUnlocked = false;
    private bool firstDeathUnlocked = false;
    private bool firstBossUnlocked = false;
    private bool secondBossUnlocked = false;
    private bool completedGameUnlocked = false;
    private bool tenKillsUnlocked = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(status =>
        {
            isAuthenticated = status == SignInStatus.Success;
            Debug.Log("Google Play sign-in result: " + status);
        });
    }

    public void ReportScore(long score)
    {
        if (!isAuthenticated) return;

        Social.ReportScore(score, GPGSIds.leaderboard_high_score, success =>
        {
            Debug.Log("Leaderboard submit: " + success + " | score: " + score);
        });
    }

    public void UnlockAchievement(string achievementId)
    {
        if (!isAuthenticated) return;

        Social.ReportProgress(achievementId, 100.0f, success =>
        {
            Debug.Log("Achievement unlock: " + achievementId + " | " + success);
        });
    }

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
            UnlockAchievement(GPGSIds.achievement_ten_kills);
        }
    }

    public void RecordDeath()
    {
        if (firstDeathUnlocked) return;

        firstDeathUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_first_death);
    }

    public void RecordFirstBossDefeated()
    {
        if (firstBossUnlocked) return;

        firstBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_first_boss);
    }

    public void RecordSecondBossDefeated()
    {
        if (secondBossUnlocked) return;

        secondBossUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_second_boss);
    }

    public void RecordGameCompleted()
    {
        if (completedGameUnlocked) return;

        completedGameUnlocked = true;
        UnlockAchievement(GPGSIds.achievement_completed_game);
    }

    public void ShowAchievementsUI()
    {
        if (!isAuthenticated) return;
        Social.ShowAchievementsUI();
    }

    public void ShowLeaderboardUI()
    {
        if (!isAuthenticated) return;
        Social.ShowLeaderboardUI();
    }
}
