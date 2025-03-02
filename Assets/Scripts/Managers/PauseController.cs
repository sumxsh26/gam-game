using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; } // Singleton instance

    [Header("Pause UI")]
    public Canvas pauseMenuCanvas; // Assign in Inspector
    public GameObject gameplayUI;  // Reference to gameplay UI

    public static bool isPaused = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure pause menu starts hidden
        if (pauseMenuCanvas != null && pauseMenuCanvas.gameObject.activeSelf)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputManager.PauseWasPressed)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        //// Hide gameplay UI if assigned
        //if (gameplayUI != null)
        //{
        //    gameplayUI.SetActive(false);
        //}

        // Show pause menu
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(true);
        }

        Time.timeScale = 0f; // Pause game time
    }

    public void UnpauseGame()
    {
        isPaused = false;

        //// Show gameplay UI
        //if (gameplayUI != null)
        //{
        //    gameplayUI.SetActive(true);
        //}

        // Hide pause menu
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
        }

        Time.timeScale = 1f; // Resume game time
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is reset before loading the main menu
        StartCoroutine(QuitWithDelay(1.0f)); // Add delay before quitting
    }

    private IEnumerator QuitWithDelay(float delay)
    {
        Debug.Log("Returning to main menu in " + delay + " seconds...");

        // Wait for the delay
        yield return new WaitForSecondsRealtime(delay);

        SceneManager.LoadScene("Menu"); // Load the main menu scene
    }
}
