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

    public bool isGameOver = false;

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


    void WhenPlayerDies()
    {
        Debug.Log("[GameController] WhenPlayerDies triggered");

        isGameOver = true;

        // Hide Gameplay UI if the game is paused or not
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
            Debug.Log("[GameController] Gameplay UI hidden");
        }

        StartCoroutine(ShowGameOverAfterDelay());
    }


    private IEnumerator ShowGameOverAfterDelay()
    {
        float deathAnimationDuration = GetPlayerDeathAnimationDuration();
        Debug.Log("[GameController] Waiting " + deathAnimationDuration + "s before showing game over screen");

        yield return new WaitForSeconds(deathAnimationDuration);

        GameOverCanvas.gameObject.SetActive(true);
        Debug.Log("[GameController] Game Over screen shown");
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
        yield return new WaitForSeconds(delay);

        Debug.Log("[GameController] Retry pressed - calling RespawnAtCheckpoint");

        isGameOver = false;

        // Hide Game Over screen
        if (GameOverCanvas != null)
        {
            GameOverCanvas.gameObject.SetActive(false);
        }

        // Re-enable gameplay UI
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(true);
        }

        // Respawn the player manually
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RespawnAtCheckpoint();
        }
        else
        {
            Debug.LogError("[GameController] CheckpointManager is missing!");
        }

        // Re-subscribe to death event so it triggers again next time
        if (playerMovement != null)
        {
            playerMovement.PlayerDied += WhenPlayerDies;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindPlayer();
    }


    public void RebindPlayer()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            playerMovement.PlayerDied -= WhenPlayerDies; // avoid double binding
            playerMovement.PlayerDied += WhenPlayerDies;
            Debug.Log("[GameController] Player rebound and PlayerDied subscribed");
        }
        else
        {
            Debug.LogWarning("[GameController] Could not find player to rebind");
        }
    }

}
