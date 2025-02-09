using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public Canvas GameOverCanvas;
    public Text TimerText;

    public static bool isGameOver = false; // Static flag for TimerManagerScript
    public PlayerController PlayerController => playerController; // Public getter

    private void Awake()
    {
        if (playerController != null)
        {
            //subscribe
            playerController.PlayerDied += WhenPlayerDies;
        }

        if (GameOverCanvas.gameObject.activeSelf)
        {
            GameOverCanvas.gameObject.SetActive(false);
        }
    }

    // when player dies
    void WhenPlayerDies()
    {
        isGameOver = true;  // Signal TimerManagerScript to stop timer

        GameOverCanvas.gameObject.SetActive(true);

        //timer
        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        float seconds = Time.timeSinceLevelLoad % 60;

        // in minutes
        //TimerText.text = $"You Lasted: {minutes:00}:{seconds:00.00} minutes";

        // in seconds
        TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";

        if (playerController != null)
        {
            //Unsubscribe
            playerController.PlayerDied -= WhenPlayerDies;
            //SceneManager.LoadScene("GameOver");
        }
    }

    public void RetryClicked()
    {
        isGameOver = false;  // Reset game over state
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
