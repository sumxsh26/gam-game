//using TMPro;
//using UnityEngine;

//public class TimerManagerScript : MonoBehaviour
//{
//    [SerializeField] TextMeshProUGUI countdownTimerText;
//    [SerializeField] float remainingTime;

//    void Update()
//    {
//        if (GameControllerScript.isGameOver)
//            return; // Stop updating timer if game over

//        if (remainingTime > 0)
//        {
//            remainingTime -= Time.deltaTime;
//        }
//        else if (remainingTime < 0)
//        {
//            remainingTime = 0;
//            countdownTimerText.color = Color.red;

//            // Trigger the PlayerDied event safely
//            GameControllerScript.Instance?.PlayerController?.TriggerPlayerDeath();
//        }

//        int minutes = Mathf.FloorToInt(remainingTime / 60);
//        int seconds = Mathf.FloorToInt(remainingTime % 60);
//        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
//    }
//}

using TMPro;
using UnityEngine;

public class TimerManagerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownTimerText;
    [SerializeField] float remainingTime = 300f;  // Set a default starting time (e.g., 5 minutes)

    void Start()
    {
        // If the remainingTime is set to zero or an invalid value, reset it to the default
        if (remainingTime <= 0)
        {
            remainingTime = 300f; // Set to a default value, e.g., 5 minutes
        }
    }

    void Update()
    {
        if (GameControllerScript.isGameOver)
            return; // Stop updating timer if game over

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            countdownTimerText.color = Color.red;

            // Trigger the PlayerDied event safely
            GameControllerScript.Instance?.PlayerController?.TriggerPlayerDeath();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

