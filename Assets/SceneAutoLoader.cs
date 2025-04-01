using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoLoader : MonoBehaviour
{
    [Tooltip("Name of the scene to load after the delay")]
    [SerializeField] private string sceneToLoad = "NextScene";

    [Tooltip("Delay before loading the scene (in seconds)")]
    [SerializeField] private float delay = 10f;

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delay);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("[SceneAutoLoader] No scene name specified.");
        }
    }
}
