using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int health = 3;

    [Header("Shield")]
    public int maxShieldHits = 3;
    public int shieldHits = 0;

    private bool isDead;
    private HUD hud;

    [SerializeField] private ParticleSystem shieldFX;

    void Start()
    {
        hud = FindFirstObjectByType<HUD>();

        UpdateShieldVisual();
        RefreshHUD();
    }

    // called whenever the player gets hit
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // shield absorbs damage first
        if (shieldHits > 0)
        {
            shieldHits--;
            UpdateShieldVisual();
            RefreshHUD();

            // small visual feedback when shield blocks a hit
            shieldFX.Emit(20);
            return;
        }

        // apply real health damage
        health -= damage;
        RefreshHUD();

        if (health <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        // clamp so health never exceeds max
        health = Mathf.Min(maxHealth, health + amount);
        RefreshHUD();
    }

    public void AddShield(int hits)
    {
        if (isDead) return;

        shieldHits = Mathf.Min(maxShieldHits, shieldHits + hits);
        UpdateShieldVisual();
        RefreshHUD();
    }

    private void RefreshHUD()
    {
        if (hud == null) return;

        hud.SetHealth(health, maxHealth);
        hud.SetShield(shieldHits, maxShieldHits);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        var deathUI = FindFirstObjectByType<DeathUI>();

        if (deathUI != null)
            deathUI.ShowDeath();
        else
            Time.timeScale = 0f;

        // disable player instead of destroying so references don't break
        gameObject.SetActive(false);

        SoundManager.Instance.Play(SoundManager.Instance.playerDeath);
    }

    void UpdateShieldVisual()
    {
        if (shieldFX == null) return;

        // shield particle plays only while shield is active
        if (shieldHits > 0)
        {
            if (!shieldFX.isPlaying)
                shieldFX.Play();
        }
        else
        {
            if (shieldFX.isPlaying)
                shieldFX.Stop();
        }
    }
}