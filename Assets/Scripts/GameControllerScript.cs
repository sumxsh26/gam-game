using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance { get; private set; }

    [SerializeField] private PlayerController playerController;
    public Canvas GameOverCanvas;
    public Text TimerText;

    [Header("Gameplay UI")]
    public GameObject gameplayUI; // Reference to the Gameplay UI parent

    public static bool isGameOver = false;
    public PlayerController PlayerController => playerController;

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

        if (playerController != null)
        {
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
        isGameOver = true;

        // hide Gameplay UI
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        GameOverCanvas.gameObject.SetActive(true);

        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        float seconds = Time.timeSinceLevelLoad % 60;

        TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";

        if (playerController != null)
        {
            playerController.PlayerDied -= WhenPlayerDies;
        }
    }

    public void RetryClicked()
    {
        // 1.5 second delay to allow gameplay UI to load up before restarting
        StartCoroutine(RetryWithDelay(1.5f)); 
    }

    private IEnumerator RetryWithDelay(float delay)
    {
        
        Debug.Log("Restarting game in " + delay + " seconds...");

        // wait for the delay
        yield return new WaitForSeconds(delay);

        // reset the game over state
        isGameOver = false;

        // reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
