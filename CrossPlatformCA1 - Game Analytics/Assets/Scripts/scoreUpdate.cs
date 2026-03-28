using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreUpdate : MonoBehaviour
{
    private GameManager gm;
    private int points;
    private bool awardScore = true;

    // called by the spawner right after creating the enemy
    public void Init(GameManager gameManager, int scorePoints)
    {
        gm = gameManager;
        points = scorePoints;
    }

    // used when enemies are force-removed
    public void Suppress()
    {
        awardScore = false;
    }

    private void OnDestroy()
    {
        // only award score if allowed
        if (!awardScore) return;

        if (gm != null && points > 0)
            gm.AddScore(points);
    }
}