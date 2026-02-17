using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    // Panel that appears when the player dies
    [SerializeField] private GameObject deathPanel;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Called when the player dies, Shows UI + pauses the game
    public void ShowDeath()
    {
        // Enable the death screen UI
        if (deathPanel != null)
            deathPanel.SetActive(true);

        // Pause gameplay
        Time.timeScale = 0f;
    }

    // Reload current level, Used for the retry button
    public void Retry()
    {
        // Resume time before loading otherwise next scene stays frozen
        Time.timeScale = 1f;

        // Reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Return to main menu
    public void GoToMainMenu()
    {
        // Always unpause before switching scenes
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Quit the application
    // (Only works in build, not inside Unity editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}