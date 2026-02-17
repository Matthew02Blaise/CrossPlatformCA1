using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Called when the player beats the game
    // Shows win screen + pauses gameplay
    public void ShowWin()
    {
        // Enable win UI
        winPanel.SetActive(true);

        // Freeze the game so nothing keeps moving in background
        Time.timeScale = 0f;
    }

    // Return to the main menu
    public void GoToMainMenu()
    {
        // Always restore time before scene change
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Quit the application
    // Only works in built game (not editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}