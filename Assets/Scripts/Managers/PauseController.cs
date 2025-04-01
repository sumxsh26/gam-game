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
        // Prevent pause input if game is over
        if (GameController.Instance != null && GameController.Instance.isGameOver) return;

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

        if (gameplayUI != null) // Disable gameplay UI when paused
        {
            gameplayUI.SetActive(false);
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

        if (gameplayUI != null) // Re-enable gameplay UI when unpaused
        {
            gameplayUI.SetActive(true);
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



