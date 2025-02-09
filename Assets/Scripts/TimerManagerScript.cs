using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerManagerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownTimerText;
    [SerializeField] float remainingTime;
    private GameControllerScript gameController;

    //3 mins = 180 secs

    void Start()
    {
        gameController = FindObjectOfType<GameControllerScript>(); // Find GameController in the scene
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControllerScript.isGameOver)
        {
            return; //stop updating timer if gameover
        }

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }

        //to prevent it from going to negative values
        else if(remainingTime < 0)
        {
            remainingTime = 0;
            countdownTimerText.color = Color.red;

            // Trigger the PlayerDied event
            if (gameController != null && gameController.PlayerController != null)
            {
                gameController.PlayerController.TriggerPlayerDeath(); // Call the method instead of invoking event
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}