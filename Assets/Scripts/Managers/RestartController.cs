////using UnityEngine;
////using UnityEngine.SceneManagement;

////public class RestartController : MonoBehaviour
////{
////    void Update()
////    {
////        if (InputManager.RestartWasPressed)
////        {
////            FullRestart();
////        }
////    }

////    private void FullRestart()
////    {
////        // Clear checkpoint before reloading
////        if (CheckpointManager.Instance != null)
////        {
////            CheckpointManager.Instance.ClearCheckpointData();
////        }

////        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
////    }
////}


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

        // Reload the scene to fully reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

