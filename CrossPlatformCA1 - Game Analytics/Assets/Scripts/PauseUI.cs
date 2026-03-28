using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused;

    void Start()
    {
        // make sure the pause menu is hidden when the game starts
        if (pausePanel != null) pausePanel.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        // pressing escape toggles pause
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        // prevents pausing if the game is already frozen by death/win screen
        if (!isPaused && Time.timeScale == 0f) return;

        isPaused = !isPaused;

        // show or hide pause UI
        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        // freeze or resume gameplay
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        // resume button from the UI
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        // always restore time before switching scenes
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        // only works in build, not in the Unity editor
        Application.Quit();
    }
}