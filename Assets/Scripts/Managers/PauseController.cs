//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PauseController : MonoBehaviour
//{
//    public static PauseController Instance { get; private set; } // Singleton instance

//    [Header("Pause UI")]
//    public Canvas pauseMenuCanvas; // Assign in Inspector
//    public GameObject gameplayUI;  // Reference to gameplay UI

//    public static bool isPaused = false;

//    private void Awake()
//    {
//        // Singleton pattern to ensure only one instance exists
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        // Ensure pause menu starts hidden
//        if (pauseMenuCanvas != null && pauseMenuCanvas.gameObject.activeSelf)
//        {
//            pauseMenuCanvas.gameObject.SetActive(false);
//        }
//    }

//    private void Update()
//    {
//        if (InputManager.PauseWasPressed)
//        {
//            TogglePause();
//        }
//    }

//    public void TogglePause()
//    {
//        if (isPaused)
//        {
//            UnpauseGame();
//        }
//        else
//        {
//            PauseGame();
//        }
//    }

//    public void PauseGame()
//    {
//        isPaused = true;

//        //// Hide gameplay UI if assigned
//        //if (gameplayUI != null)
//        //{
//        //    gameplayUI.SetActive(false);
//        //}

//        // Show pause menu
//        if (pauseMenuCanvas != null)
//        {
//            pauseMenuCanvas.gameObject.SetActive(true);
//        }

//        Time.timeScale = 0f; // Pause game time
//    }

//    public void UnpauseGame()
//    {
//        isPaused = false;

//        //// Show gameplay UI
//        //if (gameplayUI != null)
//        //{
//        //    gameplayUI.SetActive(true);
//        //}

//        // Hide pause menu
//        if (pauseMenuCanvas != null)
//        {
//            pauseMenuCanvas.gameObject.SetActive(false);
//        }

//        Time.timeScale = 1f; // Resume game time
//    }

//    public void QuitToMainMenu()
//    {
//        Time.timeScale = 1f; // Ensure time is reset before loading the main menu
//        StartCoroutine(QuitWithDelay(1.0f)); // Add delay before quitting
//    }

//    private IEnumerator QuitWithDelay(float delay)
//    {
//        Debug.Log("Returning to main menu in " + delay + " seconds...");

//        // Wait for the delay
//        yield return new WaitForSecondsRealtime(delay);

//        SceneManager.LoadScene("Menu"); // Load the main menu scene
//    }

//}


using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    [Header("Pause UI")]
    public Canvas pauseMenuCanvas;
    public GameObject gameplayUI;

    [Header("Volume Button (UI)")]
    public Image volumeButtonImage;      // Assign the UI Image from the button
    public Sprite volumeOnSprite;        // Assign "VOLUME" sprite
    public Sprite volumeMuteSprite;      // Assign "VOLUME MUTE" sprite

    [Header("Managers")]
    public RestartController restartController;

    private bool isMuted = false;

    public static bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (pauseMenuCanvas != null && pauseMenuCanvas.gameObject.activeSelf)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
        }

        AudioListener.volume = 1f;
        isMuted = false;
        UpdateVolumeButtonVisual();
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

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        isPaused = false;

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(QuitWithDelay(1.0f));
    }

    private IEnumerator QuitWithDelay(float delay)
    {
        Debug.Log("Returning to main menu in " + delay + " seconds...");
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene("Menu");
    }

    public void ToggleVolume()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateVolumeButtonVisual();
    }

    private void UpdateVolumeButtonVisual()
    {
        if (volumeButtonImage != null)
        {
            volumeButtonImage.sprite = isMuted ? volumeMuteSprite : volumeOnSprite;
        }
    }

    public void RestartFromPause()
    {
        Debug.Log("[PauseController] RestartFromPause called");

        Time.timeScale = 1f; // Ensure timescale is normal before restarting

        if (restartController != null)
        {
            restartController.FullRestart();
        }
        else
        {
            Debug.LogError("[PauseController] RestartController reference is missing!");
        }
    }

}



