using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public Canvas GameOverCanvas;
    public Text TimerText;

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
        GameOver();
    }

    //currently not working, trying to make the gameover screen open up GameOver scene
    void GameOver()
    {
        // Load the "Game Over" scene
        SceneManager.LoadScene("GameOver");
        GameOverCanvas.gameObject.SetActive(true);
        TimerText.text = "You Lasted: " + Math.Round(Time.timeSinceLevelLoad, 2);

        //unsubscribe to event
        playerController.PlayerDied -= WhenPlayerDies;
    }

    public void RetryClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
