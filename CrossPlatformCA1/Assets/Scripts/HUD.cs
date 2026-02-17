using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image shieldFill;

    // updates the player health bar
    public void SetHealth(int health, int maxHealth)
    {
        if (healthFill == null) return;

        // convert health into 0-1 range for UI fill bar
        float percentage = (maxHealth <= 0) ? 0f : (float)health / maxHealth;
        healthFill.fillAmount = Mathf.Clamp01(percentage);
    }

    // updates the shield bar and hides it when empty
    public void SetShield(int shieldHits, int maxShieldHits)
    {
        if (shieldFill == null) return;

        float pct = (maxShieldHits <= 0) ? 0f : (float)shieldHits / maxShieldHits;
        shieldFill.fillAmount = Mathf.Clamp01(pct);

        // hides the shield UI if no shield remains
        shieldFill.gameObject.SetActive(shieldHits > 0);
    }

    // updates score text
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}