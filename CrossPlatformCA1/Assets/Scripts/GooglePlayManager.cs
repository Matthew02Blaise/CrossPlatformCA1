using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
public class GooglePlayManager : MonoBehaviour
{
    void Start()
    {
        PlayGamesClientConfiguration config = new
        PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(success => {
            Debug.Log(success ? "Signed in to Google Play" : "Google Play sign-in failed");
        });
    }
}
