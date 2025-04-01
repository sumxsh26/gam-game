using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartController : MonoBehaviour
{
    void Update()
    {
        if (InputManager.RestartWasPressed)
        {
            FullRestart();
        }
    }

    public void FullRestart()
    {
        Debug.Log("[RestartController] FullRestart called");

        // Hide the Game Over canvas before reloading
        if (GameController.Instance != null && GameController.Instance.GameOverCanvas != null)
        {
            GameController.Instance.GameOverCanvas.gameObject.SetActive(false);
        }

        // Clear checkpoint data before reloading
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.ClearCheckpointData();
        }

        // Clean up persistent falling managers
        if (FallingPlatformManager.Instance != null)
        {
            FallingPlatformManager.Instance.DestroyPlatformsOnLevelTransition();
        }

        if (FallingSpikeManager.Instance != null)
        {
            FallingSpikeManager.Instance.DestroySpikesOnLevelTransition();
        }

        foreach (var door in FindObjectsByType<DoorBehaviour>(FindObjectsSortMode.None))
        {
            Destroy(door.gameObject);
        }

        var timer = Object.FindAnyObjectByType<TimerManager>();
        if (timer != null)
        {
            Destroy(timer.gameObject);
        }


        // Reload the scene to fully reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


