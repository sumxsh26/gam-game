using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [SerializeField] public float startingTime = 180f;
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
