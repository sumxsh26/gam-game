using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWonSceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load after this screen (e.g. 'Credits')")]
    public string nextSceneName = "Credits";

    [Tooltip("How long to wait before loading the next scene (in seconds)")]
    public float delayBeforeNextScene = 5f;

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delayBeforeNextScene);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("[YouWonSceneController] No scene name set!");
        }
    }
}
