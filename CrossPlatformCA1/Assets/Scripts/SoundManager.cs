using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SoundManager : MonoBehaviour
{
    // singleton
    public static SoundManager Instance;

    [SerializeField] private AudioSource sfxSource;

    [Header("Player")]
    public AudioClip playerShoot;
    public AudioClip playerDeath;

    [Header("Enemy")]
    public AudioClip enemyShoot;
    public AudioClip enemyDeath;

    public AudioClip boss2AcidSpit;
    public AudioClip boss3LaserFire;

    void Awake()
    {
        // ensures only one sound manager exists
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

    // plays sound effect
    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }
}