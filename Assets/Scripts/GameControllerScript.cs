using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance { get; private set; } // Singleton Instance

    [SerializeField] private PlayerController playerController;
    public Canvas GameOverCanvas;
    public Text TimerText;

    public static bool isGameOver = false; // Static flag for TimerManagerScript
    public PlayerController PlayerController => playerController; // Public getter

    private void Awake()
    {
        // Singleton pattern to ensure there's only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
            return;
        }

        if (playerController != null)
        {
            // Subscribe to the player's death event
            playerController.PlayerDied += WhenPlayerDies;
        }

        if (GameOverCanvas.gameObject.activeSelf)
        {
            GameOverCanvas.gameObject.SetActive(false);
        }
    }

    // When player dies
    void WhenPlayerDies()
    {
        isGameOver = true;  // Signal TimerManagerScript to stop timer

        GameOverCanvas.gameObject.SetActive(true);

        // Timer
        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        float seconds = Time.timeSinceLevelLoad % 60;

        // In seconds
        TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";

        if (playerController != null)
        {
            // Unsubscribe to avoid memory leaks
            playerController.PlayerDied -= WhenPlayerDies;
        }
    }

    public void RetryClicked()
    {
        isGameOver = false;  // Reset game over state
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
