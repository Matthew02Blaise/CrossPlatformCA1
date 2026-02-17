using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script for the main menu, which has buttons to start the game and quit the application
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string game = "game";

    public void PlayGame()
    {
        SceneManager.LoadScene(game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
