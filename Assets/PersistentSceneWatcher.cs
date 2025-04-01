using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneWatcher : MonoBehaviour
{
    private static PersistentSceneWatcher instance;

    private void Awake()
    {
        // Singleton to ensure only one exists
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene change
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameResetUtility.CleanupPersistentManagers();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
