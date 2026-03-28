using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // singleton so music keeps playing
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;

    // background tracks
    public AudioClip normalMusic;
    public AudioClip boss1Music;
    public AudioClip boss2Music;
    public AudioClip boss3Music;

    private AudioClip currentClip;

    void Awake()
    {
        // ensure only one music manager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // plays normal gameplay music
    public void PlayNormal()
    {
        Play(normalMusic);
    }

    // selects boss music based on which boss is active
    public void PlayBoss(int bossIndex)
    {
        if (bossIndex == 0) Play(boss1Music);
        else if (bossIndex == 1) Play(boss2Music);
        else if (bossIndex == 2) Play(boss3Music);
    }

    // internal play function that prevents restarting same track
    private void Play(AudioClip clip)
    {
        if (clip == null || clip == currentClip) return;

        currentClip = clip;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}