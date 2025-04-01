//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class YouWonSceneController : MonoBehaviour
//{
//    [Header("Scene Settings")]
//    [Tooltip("Name of the scene to load after this screen (e.g. 'Credits')")]
//    public string nextSceneName = "Credits";

//    [Tooltip("How long to wait before loading the next scene (in seconds)")]
//    public float delayBeforeNextScene = 5f;

//    private void Start()
//    {
//        Invoke(nameof(LoadNextScene), delayBeforeNextScene);
//    }

//    private void LoadNextScene()
//    {
//        if (!string.IsNullOrEmpty(nextSceneName))
//        {
//            SceneManager.LoadScene(nextSceneName);
//        }
//        else
//        {
//            Debug.LogError("[YouWonSceneController] No scene name set!");
//        }
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWonSceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load after this screen (e.g. 'Credits')")]
    public string nextSceneName = "Credits";

    [Tooltip("How long to wait before loading the next scene (in seconds)")]
    public float delayBeforeNextScene = 10f;

    private bool isLoading = false;

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delayBeforeNextScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isLoading)
        {
            CancelInvoke(nameof(LoadNextScene));
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        isLoading = true;

        // Clean up persistent managers before transitioning
        if (FallingPlatformManager.Instance != null)
        {
            FallingPlatformManager.Instance.DestroyPlatformsOnLevelTransition();
        }

        if (FallingSpikeManager.Instance != null)
        {
            FallingSpikeManager.Instance.DestroySpikesOnLevelTransition();
        }

        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.DestroyCheckpointsOnLevelTransition();
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }

        foreach (var door in FindObjectsByType<DoorBehaviour>(FindObjectsSortMode.None))
        {
            Destroy(door.gameObject);
        }

    }
}



