using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private string nextSceneName;
    private AsyncOperation sceneLoadOperation;

    //[SerializeField] private float videoDuration = 4.0f; // Expected video duration

    private void Start()
    {
        // Get the next scene name that was stored in PlayerPrefs
        nextSceneName = PlayerPrefs.GetString("NextLevel", "Menu"); // Default to MainMenu if not found
        Debug.Log("[DEBUG] Loading scene. Next level: " + nextSceneName);

        videoPlayer = GetComponentInChildren<VideoPlayer>();

        if (videoPlayer != null)
        {
            StartCoroutine(HandleLoadingScreen());
        }
        else
        {
            Debug.LogWarning("[DEBUG] No VideoPlayer found! Skipping to next scene.");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator HandleLoadingScreen()
    {
        // Play the video immediately
        videoPlayer.Play();

        // Start loading the next scene in the background after 3.5 seconds
        yield return new WaitForSeconds(3.8f);
        Debug.Log("[DEBUG] Preloading next scene...");
        sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        sceneLoadOperation.allowSceneActivation = false; // Prevents immediate switch

        // Wait for 0.4 more seconds (video keeps playing)
        yield return new WaitForSeconds(0.4f);

        Debug.Log("[DEBUG] Video finished. Instantly switching to the next scene.");

        // Instantly activate the next scene without fading
        sceneLoadOperation.allowSceneActivation = true;
    }
}
