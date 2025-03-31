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
//using UnityEngine;
//using TMPro;

//public class TimerManager : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI countdownTimerText;
//    [SerializeField] private float remainingTime = 180f;  // Default 3 minutes
//    //private bool isFlashing = false;
//    private AudioSource audioSource;
//    [SerializeField] private AudioClip warningBeep; // Assign in Inspector
//    private int lastPlayedSecond = -1; // Track last second audio played

//    void Start()
//    {
//        GameController.isGameOver = false; // Ensure game is active
//        if (remainingTime <= 0)
//        {
//            remainingTime = 180f; // Reset to default
//            Debug.Log("Timer initialized to: " + remainingTime);
//        }
//        audioSource = GetComponent<AudioSource>();
//    }

//    void Update()
//    {
//        if (GameController.isGameOver) return;

//        if (remainingTime > 0)
//        {
//            remainingTime -= Time.deltaTime;
//        }
//        else
//        {
//            remainingTime = 0;
//            GameController.Instance?.PlayerMovement?.TriggerPlayerDeath();
//        }

//        int minutes = Mathf.FloorToInt(remainingTime / 60);
//        int seconds = Mathf.FloorToInt(remainingTime % 60);
//        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

//        // **DRAMATIC EFFECTS START AT 10 SECONDS**
//        if (remainingTime <= 10)
//        {
//            countdownTimerText.color = Color.red;

//            // **Flashing Effect**
//            float intensity = Mathf.PingPong(Time.time * 2, 1);
//            countdownTimerText.color = new Color(1, intensity, intensity);

//            // **Scaling Effect**
//            float scale = 1 + Mathf.PingPong(Time.time * 0.3f, 0.3f);
//            countdownTimerText.transform.localScale = new Vector3(scale, scale, 1);
//        }

//        // **Play Warning Beep Only in Last 10 Seconds**
//        if (remainingTime <= 10 && seconds != lastPlayedSecond)
//        {
//            audioSource.PlayOneShot(warningBeep);
//            lastPlayedSecond = seconds; // Prevent duplicate plays
//        }
//    }
//}


//using UnityEngine;
//using TMPro;

//public class TimerManager : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI countdownTimerText;
//    [SerializeField] private float remainingTime = 180f;  // Default 3 minutes

//    private AudioSource audioSource;
//    [SerializeField] private AudioClip warningBeep; // Assign in Inspector

//    private int lastPlayedSecond = -1; // Track last second audio played

//    void Start()
//    {
//        if (GameController.Instance != null)
//        {
//            GameController.Instance.isGameOver = false;
//        }

//        if (remainingTime <= 0)
//        {
//            remainingTime = 180f; // Default time
//        }

//        lastPlayedSecond = -1;
//        countdownTimerText.color = Color.white;
//        countdownTimerText.transform.localScale = Vector3.one;

//        audioSource = GetComponent<AudioSource>();
//    }


//    void Update()
//    {
//        // Don't update timer if game is over
//        if (GameController.Instance != null && GameController.Instance.isGameOver) return;

//        if (remainingTime > 0)
//        {
//            remainingTime -= Time.deltaTime;
//        }
//        else
//        {
//            remainingTime = 0;

//            // Trigger player death when timer hits zero (safe check)
//            if (GameController.Instance?.PlayerMovement != null && GameController.Instance.isGameOver == false)
//            {
//                GameController.Instance.PlayerMovement.TriggerPlayerDeath();
//            }
//        }

//        int minutes = Mathf.FloorToInt(remainingTime / 60);
//        int seconds = Mathf.FloorToInt(remainingTime % 60);
//        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

//        // **DRAMATIC EFFECTS START AT 10 SECONDS**
//        if (remainingTime <= 10)
//        {
//            // Flashing red
//            float intensity = Mathf.PingPong(Time.time * 2, 1);
//            countdownTimerText.color = new Color(1, intensity, intensity);

//            // Pulsing scale
//            float scale = 1 + Mathf.PingPong(Time.time * 0.3f, 0.3f);
//            countdownTimerText.transform.localScale = new Vector3(scale, scale, 1);
//        }

//        // **Play warning beep in last 10 seconds (once per second)**
//        if (remainingTime <= 10 && seconds != lastPlayedSecond)
//        {
//            audioSource.PlayOneShot(warningBeep);
//            lastPlayedSecond = seconds;
//        }
//    }

//    public void ResetTimer()
//    {
//        // Reset visuals and timer value to whatever is set in the Inspector
//        countdownTimerText.color = Color.white;
//        countdownTimerText.transform.localScale = Vector3.one;
//        lastPlayedSecond = -1;

//        if (GameController.Instance != null)
//        {
//            GameController.Instance.isGameOver = false;
//        }
//    }



//}

//using UnityEngine;
//using TMPro;

//public class TimerManager : MonoBehaviour
//{
//    [SerializeField] private TextMeshProUGUI countdownTimerText;
//    [SerializeField] private float startingTime = 180f; // Set this in Inspector per scene
//    private float remainingTime;

//    private AudioSource audioSource;
//    [SerializeField] private AudioClip warningBeep;

//    private int lastPlayedSecond = -1;

//    void Start()
//    {
//        remainingTime = startingTime;
//        lastPlayedSecond = -1;
//        countdownTimerText.color = Color.white;
//        countdownTimerText.transform.localScale = Vector3.one;

//        if (GameController.Instance != null)
//        {
//            GameController.Instance.isGameOver = false;
//        }

//        audioSource = GetComponent<AudioSource>();
//    }

//    void Update()
//    {
//        if (GameController.Instance != null && GameController.Instance.isGameOver) return;

//        if (remainingTime > 0)
//        {
//            remainingTime -= Time.deltaTime;
//        }
//        else
//        {
//            remainingTime = 0;

//            if (GameController.Instance?.PlayerMovement != null && !GameController.Instance.isGameOver)
//            {
//                GameController.Instance.PlayerMovement.TriggerPlayerDeath();
//            }
//        }

//        int minutes = Mathf.FloorToInt(remainingTime / 60);
//        int seconds = Mathf.FloorToInt(remainingTime % 60);
//        countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

//        if (remainingTime <= 10)
//        {
//            float intensity = Mathf.PingPong(Time.time * 2, 1);
//            countdownTimerText.color = new Color(1, intensity, intensity);

//            float scale = 1 + Mathf.PingPong(Time.time * 0.3f, 0.3f);
//            countdownTimerText.transform.localScale = new Vector3(scale, scale, 1);
//        }

//        if (remainingTime <= 10 && seconds != lastPlayedSecond)
//        {
//            audioSource.PlayOneShot(warningBeep);
//            lastPlayedSecond = seconds;
//        }
//    }

//    public void ResetTimer()
//    {
//        remainingTime = startingTime;

//        countdownTimerText.color = Color.white;
//        countdownTimerText.transform.localScale = Vector3.one;
//        lastPlayedSecond = -1;

//        if (GameController.Instance != null)
//        {
//            GameController.Instance.isGameOver = false;
//        }
//    }
//}


using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [SerializeField] public float startingTime = 180f; // Set this in Inspector per scene
    private float remainingTime;

    private AudioSource audioSource;
    [SerializeField] private AudioClip warningBeep;

    private int lastPlayedSecond = -1;

    void Start()
    {
        remainingTime = startingTime;
        lastPlayedSecond = -1;

        // Defensive check to avoid null issues after scene reload
        if (countdownTimerText != null)
        {
            countdownTimerText.color = Color.white;
            countdownTimerText.transform.localScale = Vector3.one;
        }

        if (GameController.Instance != null)
        {
            GameController.Instance.isGameOver = false;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameController.Instance != null && GameController.Instance.isGameOver) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;

            if (GameController.Instance?.PlayerMovement != null && !GameController.Instance.isGameOver)
            {
                GameController.Instance.PlayerMovement.TriggerPlayerDeath();
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        // Safety check
        if (countdownTimerText != null)
        {
            countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (remainingTime <= 10)
            {
                float intensity = Mathf.PingPong(Time.time * 2, 1);
                countdownTimerText.color = new Color(1, intensity, intensity);

                float scale = 1 + Mathf.PingPong(Time.time * 0.3f, 0.3f);
                countdownTimerText.transform.localScale = new Vector3(scale, scale, 1);
            }
            else
            {
                countdownTimerText.color = Color.white;
                countdownTimerText.transform.localScale = Vector3.one;
            }
        }

        if (remainingTime <= 10 && seconds != lastPlayedSecond)
        {
            if (audioSource != null && warningBeep != null)
            {
                audioSource.PlayOneShot(warningBeep);
            }

            lastPlayedSecond = seconds;
        }
    }

    //public void ResetTimer()
    //{
    //    remainingTime = startingTime;
    //    lastPlayedSecond = -1;

    //    // Null check for safety after reload
    //    if (countdownTimerText != null)
    //    {
    //        countdownTimerText.color = Color.white;
    //        countdownTimerText.transform.localScale = Vector3.one;
    //        countdownTimerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(startingTime / 60), Mathf.FloorToInt(startingTime % 60));
    //    }
    //    else
    //    {
    //        Debug.LogWarning("[TimerManager] countdownTimerText is null during ResetTimer. Was the scene reloaded?");
    //    }

    //    if (GameController.Instance != null)
    //    {
    //        GameController.Instance.isGameOver = false;
    //    }
    //}

    public void ResetTimer(float customTime = -1f)
    {
        remainingTime = (customTime > 0f) ? customTime : startingTime;
        lastPlayedSecond = -1;

        if (countdownTimerText != null)
        {
            countdownTimerText.color = Color.white;
            countdownTimerText.transform.localScale = Vector3.one;
            countdownTimerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(remainingTime / 60), Mathf.FloorToInt(remainingTime % 60));
        }

        if (GameController.Instance != null)
        {
            GameController.Instance.isGameOver = false;
        }
    }


    public float GetCurrentTime()
    {
        return remainingTime;
    }

}
