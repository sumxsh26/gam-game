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

    private void FullRestart()
    {
        // Clear checkpoint before reloading
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.ClearCheckpointData();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
