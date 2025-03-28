////using System;
////using System.Collections;
////using UnityEngine;
////using UnityEngine.SceneManagement;
////using UnityEngine.UI;

////public class GameController : MonoBehaviour
////{
////    public static GameControllerScript Instance { get; private set; }

////    [SerializeField] private PlayerController playerController;
////    public Canvas GameOverCanvas;
////    public Text TimerText;

////    [Header("Gameplay UI")]
////    public GameObject gameplayUI; // Reference to the Gameplay UI parent

////    public static bool isGameOver = false;
////    public PlayerController PlayerController => playerController;

////    private void Awake()
////    {
////        if (Instance == null)
////        {
////            Instance = this;
////        }
////        else
////        {
////            Destroy(gameObject);
////            return;
////        }

////        if (playerController != null)
////        {
////            playerController.PlayerDied += WhenPlayerDies;
////        }

////        if (GameOverCanvas.gameObject.activeSelf)
////        {
////            GameOverCanvas.gameObject.SetActive(false);
////        }
////    }

////    // When player dies
////    void WhenPlayerDies()
////    {
////        isGameOver = true;

////        // hide Gameplay UI
////        if (gameplayUI != null)
////        {
////            gameplayUI.SetActive(false);
////        }

////        GameOverCanvas.gameObject.SetActive(true);

////        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
////        float seconds = Time.timeSinceLevelLoad % 60;

////        TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";

////        if (playerController != null)
////        {
////            playerController.PlayerDied -= WhenPlayerDies;
////        }
////    }

////    public void RetryClicked()
////    {
////        // 1.5 second delay to allow gameplay UI to load up before restarting
////        StartCoroutine(RetryWithDelay(1.5f)); 
////    }

////    private IEnumerator RetryWithDelay(float delay)
////    {

////        Debug.Log("Restarting game in " + delay + " seconds...");

////        // wait for the delay
////        yield return new WaitForSeconds(delay);

////        // reset the game over state
////        isGameOver = false;

////        // reload the current scene
////        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
////    }


////}


////using System;
////using System.Collections;
////using UnityEngine;
////using UnityEngine.SceneManagement;
////using UnityEngine.UI;

////public class GameController : MonoBehaviour
////{
////    public static GameController Instance { get; private set; }

////    [SerializeField] private PlayerMovement playerMovement;
////    public Canvas GameOverCanvas;
////    public Text TimerText;

////    [Header("Gameplay UI")]
////    public GameObject gameplayUI; // Reference to the Gameplay UI parent

////    public static bool isGameOver = false;
////    public PlayerMovement PlayerMovement => playerMovement;

////    private void Awake()
////    {
////        if (Instance == null)
////        {
////            Instance = this;
////        }
////        else
////        {
////            Destroy(gameObject);
////            return;
////        }

////        if (playerMovement != null)
////        {
////            playerMovement.PlayerDied += WhenPlayerDies;
////        }

////        if (GameOverCanvas.gameObject.activeSelf)
////        {
////            GameOverCanvas.gameObject.SetActive(false);
////        }
////    }

////    // When player dies
////    void WhenPlayerDies()
////    {
////        isGameOver = true;

////        // Hide Gameplay UI immediately
////        if (gameplayUI != null)
////        {
////            gameplayUI.SetActive(false);
////        }

////        // Start coroutine to delay the Game Over screen
////        StartCoroutine(ShowGameOverAfterDelay());

////        if (playerMovement != null)
////        {
////            playerMovement.PlayerDied -= WhenPlayerDies;
////        }
////    }

////    private IEnumerator ShowGameOverAfterDelay()
////    {
////        float deathAnimationDuration = GetPlayerDeathAnimationDuration();
////        yield return new WaitForSeconds(deathAnimationDuration);

////        GameOverCanvas.gameObject.SetActive(true);

////        //int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
////        //float seconds = Time.timeSinceLevelLoad % 60;

////        //TimerText.text = "You Lasted: " + Time.timeSinceLevelLoad.ToString("00.00") + " seconds";
////    }

////    private float GetPlayerDeathAnimationDuration()
////    {
////        if (playerMovement != null && playerMovement.GetComponent<Animator>() != null)
////        {
////            Animator animator = playerMovement.GetComponent<Animator>();
////            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

////            foreach (var clip in clipInfo)
////            {
////                if (clip.clip.name.Contains("Death")) // Adjust based on the animation name
////                {
////                    return clip.clip.length;
////                }
////            }
////        }

////        // Default fallback duration if no animation is found
////        return 1.5f;
////    }

////    public void RetryClicked()
////    {
////        // 1.5 second delay to allow gameplay UI to load up before restarting
////        StartCoroutine(RetryWithDelay(0.5f));
////    }

////    private IEnumerator RetryWithDelay(float delay)
////    {
////        Debug.Log("Restarting game in " + delay + " seconds...");

////        // Wait for the delay
////        yield return new WaitForSeconds(delay);

////        // Reset the game over state
////        isGameOver = false;

////        // Reload the current scene
////        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
////    }
////}


//// checkpoint
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

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(transform.root.gameObject);

    //        isGameOver = false; //  Reset on creation
    //        GameOverCanvas.gameObject.SetActive(false); //  Hide Game Over UI at scene start
    //    }
    //    else if (Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //}

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
        Debug.Log("[GameController] WhenPlayerDies triggered");

        isGameOver = true;

        // Hide Gameplay UI
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
            Debug.Log("[GameController] Gameplay UI hidden");
        }

        StartCoroutine(ShowGameOverAfterDelay());

        // DO NOT unsubscribe here!
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

// with restart
//using System;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class GameController : MonoBehaviour
//{
//    public static GameController Instance { get; private set; }

//    [SerializeField] private PlayerMovement playerMovement;
//    public Canvas GameOverCanvas;
//    public Text TimerText;

//    [Header("Gameplay UI")]
//    public GameObject gameplayUI; // Reference to the Gameplay UI parent

//    public bool isGameOver = false;

//    public PlayerMovement PlayerMovement => playerMovement;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(transform.root.gameObject);

//            isGameOver = false; //  Reset on creation
//            GameOverCanvas.gameObject.SetActive(false); //  Hide Game Over UI at scene start
//        }
//        else if (Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//    }

//    // When player dies
//    void WhenPlayerDies()
//    {
//        Debug.Log("[GameController] WhenPlayerDies triggered");

//        isGameOver = true;

//        // Hide Gameplay UI
//        if (gameplayUI != null)
//        {
//            gameplayUI.SetActive(false);
//            Debug.Log("[GameController] Gameplay UI hidden");
//        }

//        StartCoroutine(ShowGameOverAfterDelay());
//    }

//    private IEnumerator ShowGameOverAfterDelay()
//    {
//        float deathAnimationDuration = GetPlayerDeathAnimationDuration();
//        Debug.Log("[GameController] Waiting " + deathAnimationDuration + "s before showing game over screen");

//        yield return new WaitForSeconds(deathAnimationDuration);

//        GameOverCanvas.gameObject.SetActive(true);
//        Debug.Log("[GameController] Game Over screen shown");
//    }

//    private float GetPlayerDeathAnimationDuration()
//    {
//        if (playerMovement != null && playerMovement.GetComponent<Animator>() != null)
//        {
//            Animator animator = playerMovement.GetComponent<Animator>();
//            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

//            foreach (var clip in clipInfo)
//            {
//                if (clip.clip.name.Contains("Death")) // Adjust based on the animation name
//                {
//                    return clip.clip.length;
//                }
//            }
//        }

//        // Default fallback duration if no animation is found
//        return 1.5f;
//    }

//    // Retry button clicked
//    public void RetryClicked()
//    {
//        // 1.5 second delay to allow gameplay UI to load up before restarting
//        StartCoroutine(RetryWithDelay(0.5f));
//    }

//    private IEnumerator RetryWithDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);

//        Debug.Log("[GameController] Retry pressed - calling RespawnAtCheckpoint");

//        isGameOver = false;

//        // Hide Game Over screen
//        if (GameOverCanvas != null)
//        {
//            GameOverCanvas.gameObject.SetActive(false);
//        }

//        // Re-enable gameplay UI
//        if (gameplayUI != null)
//        {
//            gameplayUI.SetActive(true);
//        }

//        // Respawn the player manually
//        if (CheckpointManager.Instance != null)
//        {
//            CheckpointManager.Instance.RespawnAtCheckpoint();
//        }
//        else
//        {
//            Debug.LogError("[GameController] CheckpointManager is missing!");
//        }

//        // Re-subscribe to death event so it triggers again next time
//        if (playerMovement != null)
//        {
//            playerMovement.PlayerDied += WhenPlayerDies;
//        }
//    }

//    public void FullRestart()
//    {
//        Debug.Log("[GameController] FullRestart() called");

//        // Reset state manually
//        isGameOver = false;

//        if (GameOverCanvas != null)
//        {
//            Debug.Log("[GameController] Hiding GameOverCanvas.");
//            GameOverCanvas.gameObject.SetActive(false);
//        }

//        if (CheckpointManager.Instance != null)
//        {
//            Debug.Log("[GameController] Clearing checkpoint data.");
//            CheckpointManager.Instance.ClearCheckpointData();
//        }

//        // Reload the scene
//        Debug.Log("[GameController] Reloading scene: " + SceneManager.GetActiveScene().name);
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }





//    private void OnEnable()
//    {
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnDisable()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }


//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        RebindPlayer();

//        // Refresh GameOverCanvas reference after reload
//        GameOverCanvas = GameObject.Find("GameOverCanvas")?.GetComponent<Canvas>();
//        if (GameOverCanvas != null)
//        {
//            GameOverCanvas.gameObject.SetActive(false); // just in case
//            Debug.Log("[GameController] GameOverCanvas rebound and hidden.");
//        }
//        else
//        {
//            Debug.LogWarning("[GameController] Could not find GameOverCanvas after scene reload.");
//        }
//    }



//    public void RebindPlayer()
//    {
//        playerMovement = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMovement>();

//        if (playerMovement != null)
//        {
//            playerMovement.PlayerDied -= WhenPlayerDies; // avoid double binding
//            playerMovement.PlayerDied += WhenPlayerDies;
//            Debug.Log("[GameController] Player rebound and PlayerDied subscribed");
//        }
//        else
//        {
//            Debug.LogWarning("[GameController] Could not find player to rebind");
//        }
//    }

//    // Update method to check if 'R' key is pressed for restart
//    private void Update()
//    {
//        if (InputManager.RestartWasPressed)
//        {
//            Debug.Log("[GameController] Restart input detected.");
//            FullRestart();
//        }
//    }


//}


// with pause 
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class GameController : MonoBehaviour
//{
//    public static GameController Instance { get; private set; }

//    [SerializeField] private PlayerMovement playerMovement;
//    public Canvas GameOverCanvas;
//    public Text TimerText;

//    [Header("Gameplay UI")]
//    public GameObject gameplayUI; // Reference to the Gameplay UI parent

//    [Header("Pause UI")]
//    public Canvas pauseMenuCanvas;
//    public static bool isPaused = false;


//    public bool isGameOver = false;

//    public PlayerMovement PlayerMovement => playerMovement;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(transform.root.gameObject);

//            isGameOver = false;
//            isPaused = false;
//            Time.timeScale = 1f;

//            string currentScene = SceneManager.GetActiveScene().name;

//            if (!IsGameplayScene(currentScene))
//            {
//                GameOverCanvas?.gameObject.SetActive(false);
//                pauseMenuCanvas?.gameObject.SetActive(false);
//            }
//        }
//        else if (Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//    }



//    // Update method to check if 'R' key is pressed for restart
//    private void Update()
//    {
//        if (InputManager.RestartWasPressed)
//        {
//            Debug.Log("[GameController] Restart input detected.");
//            FullRestart();
//        }

//        if (InputManager.PauseWasPressed)
//        {
//            TogglePause();
//        }

//    }


//    // When player dies
//    void WhenPlayerDies()
//    {
//        Debug.Log("[GameController] WhenPlayerDies triggered");

//        isGameOver = true;

//        // Hide Gameplay UI
//        if (gameplayUI != null)
//        {
//            gameplayUI.SetActive(false);
//            Debug.Log("[GameController] Gameplay UI hidden");
//        }

//        StartCoroutine(ShowGameOverAfterDelay());
//    }

//    private IEnumerator ShowGameOverAfterDelay()
//    {
//        float deathAnimationDuration = GetPlayerDeathAnimationDuration();
//        Debug.Log("[GameController] Waiting " + deathAnimationDuration + "s before showing game over screen");

//        yield return new WaitForSeconds(deathAnimationDuration);

//        GameOverCanvas.gameObject.SetActive(true);
//        Debug.Log("[GameController] Game Over screen shown");
//    }

//    private float GetPlayerDeathAnimationDuration()
//    {
//        if (playerMovement != null && playerMovement.GetComponent<Animator>() != null)
//        {
//            Animator animator = playerMovement.GetComponent<Animator>();
//            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

//            foreach (var clip in clipInfo)
//            {
//                if (clip.clip.name.Contains("Death")) // Adjust based on the animation name
//                {
//                    return clip.clip.length;
//                }
//            }
//        }

//        // Default fallback duration if no animation is found
//        return 1.5f;
//    }

//    // Retry button clicked
//    public void RetryClicked()
//    {
//        // 1.5 second delay to allow gameplay UI to load up before restarting
//        StartCoroutine(RetryWithDelay(0.5f));
//    }

//    private IEnumerator RetryWithDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);

//        Debug.Log("[GameController] Retry pressed - calling RespawnAtCheckpoint");

//        isGameOver = false;

//        // Hide Game Over screen
//        if (GameOverCanvas != null)
//        {
//            GameOverCanvas.gameObject.SetActive(false);
//        }

//        // Re-enable gameplay UI
//        if (gameplayUI != null)
//        {
//            gameplayUI.SetActive(true);
//        }

//        // Respawn the player manually
//        if (CheckpointManager.Instance != null)
//        {
//            CheckpointManager.Instance.RespawnAtCheckpoint();
//        }
//        else
//        {
//            Debug.LogError("[GameController] CheckpointManager is missing!");
//        }

//        // Re-subscribe to death event so it triggers again next time
//        if (playerMovement != null)
//        {
//            playerMovement.PlayerDied += WhenPlayerDies;
//        }
//    }

//    public void FullRestart()
//    {
//        Debug.Log("[GameController] FullRestart() called");

//        if (CheckpointManager.Instance != null)
//        {
//            Debug.Log("[GameController] Clearing checkpoint data.");
//            CheckpointManager.Instance.ClearCheckpointData();
//        }

//        // Ensure Game Over and Pause are off
//        if (GameOverCanvas != null) GameOverCanvas.gameObject.SetActive(false);
//        if (pauseMenuCanvas != null) pauseMenuCanvas.gameObject.SetActive(false);
//        isGameOver = false;
//        isPaused = false;
//        Time.timeScale = 1f;

//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void TogglePause()
//    {
//        if (isPaused)
//            UnpauseGame();
//        else
//            PauseGame();
//    }

//    public void PauseGame()
//    {
//        isPaused = true;

//        if (pauseMenuCanvas != null)
//        {
//            pauseMenuCanvas.gameObject.SetActive(true);
//        }

//        Time.timeScale = 0f;
//    }

//    public void UnpauseGame()
//    {
//        isPaused = false;

//        if (pauseMenuCanvas != null)
//        {
//            pauseMenuCanvas.gameObject.SetActive(false);
//        }

//        Time.timeScale = 1f;
//    }

//    public void QuitToMainMenu()
//    {
//        Time.timeScale = 1f;
//        StartCoroutine(QuitWithDelay(1.0f));
//    }

//    private IEnumerator QuitWithDelay(float delay)
//    {
//        Debug.Log("Returning to main menu in " + delay + " seconds...");
//        yield return new WaitForSecondsRealtime(delay);
//        SceneManager.LoadScene("Menu");
//    }




//    private void OnEnable()
//    {
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnDisable()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        RebindPlayer();

//        // Rebind and hide GameOverCanvas if it exists in the scene
//        var goCanvasObj = GameObject.Find("GameOverCanvas");
//        if (goCanvasObj != null)
//        {
//            GameOverCanvas = goCanvasObj.GetComponent<Canvas>();
//            GameOverCanvas.gameObject.SetActive(false);
//            Debug.Log("[GameController] GameOverCanvas rebound and hidden.");
//        }
//        else
//        {
//            GameOverCanvas = null;
//            Debug.LogWarning("[GameController] GameOverCanvas not found in scene: " + scene.name);
//        }

//        // Rebind and hide PauseMenuCanvas if it exists in the scene
//        var pauseCanvasObj = GameObject.Find("PauseCanvas");
//        if (pauseCanvasObj != null)
//        {
//            pauseMenuCanvas = pauseCanvasObj.GetComponent<Canvas>();
//            pauseMenuCanvas.gameObject.SetActive(false);
//            Debug.Log("[GameController] PauseMenuCanvas rebound and hidden.");
//        }
//        else
//        {
//            pauseMenuCanvas = null;
//            Debug.LogWarning("[GameController] PauseMenuCanvas not found in scene: " + scene.name);
//        }
//    }



//    private readonly HashSet<string> gameplayScenes = new HashSet<string>
//    {
//        "Tutorial", "Level1", "Level2", "BossFight" // Add your gameplay scene names here
//    };

//    private bool IsGameplayScene(string sceneName)
//    {
//        return gameplayScenes.Contains(sceneName);
//    }


//    public void RebindPlayer()
//    {
//        playerMovement = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMovement>();

//        if (playerMovement != null)
//        {
//            playerMovement.PlayerDied -= WhenPlayerDies; // avoid double binding
//            playerMovement.PlayerDied += WhenPlayerDies;
//            Debug.Log("[GameController] Player rebound and PlayerDied subscribed");
//        }
//        else
//        {
//            Debug.LogWarning("[GameController] Could not find player to rebind");
//        }
//    }

//    public void GoToMainMenu()
//    {
//        SceneManager.LoadScene("Menu");
//    }


//}
