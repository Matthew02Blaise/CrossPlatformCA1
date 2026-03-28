using UnityEngine;
using GameAnalyticsSDK;

public class AnalyticsInitialiser : MonoBehaviour
{
    private static bool initialized;

    void Awake()
    {
        if (initialized)
        {
            Destroy(gameObject);
            return;
        }

        initialized = true;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GameAnalytics.Initialize();
    }
}