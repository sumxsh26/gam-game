using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer cutscenePlayer; // Assign in Inspector
    public string nextSceneName = "Instructions"; // Change if needed
    public float fadeDuration = 1.5f; // Time for fade effect

    private CanvasGroup fadePanel;

    private void Start()
    {
        // Find the FadePanel in the scene
        GameObject fadePanelObj = GameObject.Find("FadePanel");

        if (fadePanelObj != null)
        {
            fadePanel = fadePanelObj.GetComponent<CanvasGroup>();

            if (fadePanel == null)
            {
                Debug.LogError("FadePanel found, but no CanvasGroup attached! Add a CanvasGroup component.");
                return;
            }

            fadePanel.gameObject.SetActive(true); // Ensure it's active at the start
            fadePanel.alpha = 1f; // Start fully black

            StartCoroutine(FadeIn()); // Fade in when the cutscene starts
        }
        else
        {
            Debug.LogError("FadePanel not found in scene! Make sure it exists in the UI.");
        }

        // Ensure a VideoPlayer is assigned
        if (cutscenePlayer != null)
        {
            cutscenePlayer.loopPointReached += OnCutsceneEnd;
        }
        else
        {
            Debug.LogError("No VideoPlayer assigned in CutsceneManager!");
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Fade from black to normal
            yield return null;
        }

        fadePanel.alpha = 0f; // Ensure it's fully transparent
        fadePanel.gameObject.SetActive(false); // Hide panel
        cutscenePlayer.Play(); // Start cutscene after fade-in
    }

    private void OnCutsceneEnd(VideoPlayer vp)
    {
        Debug.Log("Cutscene finished! Starting fade-out before scene change...");
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.gameObject.SetActive(true); // Show the panel
        float elapsedTime = 0f;
        fadePanel.alpha = 0f; // Start transparent

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Fade to black
            yield return null;
        }

        fadePanel.alpha = 1f; // Ensure it's fully black
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
