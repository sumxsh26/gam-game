//using System;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class GameController : MonoBehaviour
//{
//    public static GameControllerScript Instance { get; private set; }

//    [SerializeField] private PlayerController playerController;
//    public Canvas GameOverCanvas;
//    public Text TimerText;

//    [Header("Gameplay UI")]
//    public GameObject gameplayUI; // Reference to the Gameplay UI parent

//    public static bool isGameOver = false;
//    public PlayerController PlayerController => playerController;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        if (playerController != null)
//        {
//            playerController.PlayerDied += WhenPlayerDies;
//        }

//        if (GameOverCanvas.gameObject.activeSelf)
//        {
//            GameOverCanvas.gameObject.SetActive(false);
//        }
//    }

//    // When player dies
//    void WhenPlayerDies()
//    {
//        isGameOver = true;

//        // hide Gameplay UI
//        if (gameplayUI != null)
//        {
//            gameplayUI.SetActive(false);
//        }

//        GameOverCanvas.gameObject.SetActive(true);

//        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
//        float seconds = Time.timeSinceLevelLoad % 60;

//        TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";

//        if (playerController != null)
//        {
//            playerController.PlayerDied -= WhenPlayerDies;
//        }
//    }

//    public void RetryClicked()
//    {
//        // 1.5 second delay to allow gameplay UI to load up before restarting
//        StartCoroutine(RetryWithDelay(1.5f)); 
//    }

//    private IEnumerator RetryWithDelay(float delay)
//    {

//        Debug.Log("Restarting game in " + delay + " seconds...");

//        // wait for the delay
//        yield return new WaitForSeconds(delay);

//        // reset the game over state
//        isGameOver = false;

//        // reload the current scene
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }


//}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;
    public Canvas GameOverCanvas;
    public Text TimerText;

    [Header("Gameplay UI")]
    public GameObject gameplayUI; // Reference to the Gameplay UI parent

    public static bool isGameOver = false;
    public PlayerMovement PlayerMovement => playerMovement;

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

        if (playerMovement != null)
        {
            playerMovement.PlayerDied += WhenPlayerDies;
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

        // Hide Gameplay UI immediately
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        // Start coroutine to delay the Game Over screen
        StartCoroutine(ShowGameOverAfterDelay());

        if (playerMovement != null)
        {
            playerMovement.PlayerDied -= WhenPlayerDies;
        }
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        float deathAnimationDuration = GetPlayerDeathAnimationDuration();
        yield return new WaitForSeconds(deathAnimationDuration);

        GameOverCanvas.gameObject.SetActive(true);

        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        float seconds = Time.timeSinceLevelLoad % 60;

        TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";
    }

    private float GetPlayerDeathAnimationDuration()
    {
        if (playerMovement != null && playerMovement.GetComponent<Animator>() != null)
        {
            Animator animator = playerMovement.GetComponent<Animator>();
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

            foreach (var clip in clipInfo)
            {
                if (clip.clip.name.Contains("Death")) // Adjust based on the animation name
                {
                    return clip.clip.length;
                }
            }
        }

        // Default fallback duration if no animation is found
        return 1.5f;
    }

    public void RetryClicked()
    {
        // 1.5 second delay to allow gameplay UI to load up before restarting
        StartCoroutine(RetryWithDelay(0.5f));
    }

    private IEnumerator RetryWithDelay(float delay)
    {
        Debug.Log("Restarting game in " + delay + " seconds...");

        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Reset the game over state
        isGameOver = false;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
