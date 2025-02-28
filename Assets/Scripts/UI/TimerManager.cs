// version without dramatic UI
/*using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownTimerText;
    [SerializeField] float remainingTime = 300f;  // Set a default starting time (e.g., 5 minutes)

    void Awake()
    {
        if (remainingTime <= 0)
        {
            remainingTime = 180f; // Set to 5 minutes at the start
            Debug.Log("Timer initialised to: " + remainingTime);
        }
    }

    void Start()
    {
        GameController.isGameOver = false; // Ensure the game starts properly
    }

    void Update()
    {
        if (GameController.isGameOver)
            return; // Stop updating timer if game over

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // Change text color to red when 30 seconds or less remain
            if (remainingTime <= 30)
            {
                countdownTimerText.color = Color.red;
            }
        }
        else
        {
            remainingTime = 0;

            // Ensure the text stays red when the timer reaches 0
            countdownTimerText.color = Color.red;

            // Trigger the PlayerDied event safely
            GameController.Instance?.PlayerController?.TriggerPlayerDeath();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}*/

// version with dramatic UI, flashing, audio, scaling
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [SerializeField] private float remainingTime = 180f;  // Default 3 minutes
    //private bool isFlashing = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip warningBeep; // Assign in Inspector
    private int lastPlayedSecond = -1; // Track last second audio played

    void Start()
    {
        GameController.isGameOver = false; // Ensure game is active
        if (remainingTime <= 0)
        {
            remainingTime = 180f; // Reset to default
            Debug.Log("Timer initialized to: " + remainingTime);
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameController.isGameOver) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            GameController.Instance?.PlayerController?.TriggerPlayerDeath();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // **DRAMATIC EFFECTS START AT 10 SECONDS**
        if (remainingTime <= 10)
        {
            countdownTimerText.color = Color.red;

            // **Flashing Effect**
            float intensity = Mathf.PingPong(Time.time * 2, 1);
            countdownTimerText.color = new Color(1, intensity, intensity);

            // **Scaling Effect**
            float scale = 1 + Mathf.PingPong(Time.time * 0.3f, 0.3f);
            countdownTimerText.transform.localScale = new Vector3(scale, scale, 1);
        }

        // **Play Warning Beep Only in Last 10 Seconds**
        if (remainingTime <= 10 && seconds != lastPlayedSecond)
        {
            audioSource.PlayOneShot(warningBeep);
            lastPlayedSecond = seconds; // Prevent duplicate plays
        }
    }
}
