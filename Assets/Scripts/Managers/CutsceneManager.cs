using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro; // Import TextMeshPro namespace

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer cutscenePlayer; // Assign in Inspector
    public string nextSceneName = "Instructions"; // Set next scene name
    public float fadeDuration = 1.5f; // Time for fade effect
    public float skipDelay = 1.0f; // Delay after fade before skipping

    private CanvasGroup fadePanel;
    private TextMeshProUGUI spaceToSkipText; // Updated to TextMeshProUGUI
    private bool isSkipping = false;

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

        // Find "Press Space to Skip" text (TextMeshProUGUI)
        GameObject skipTextObj = GameObject.Find("SpaceToSkip");

        if (skipTextObj != null)
        {
            spaceToSkipText = skipTextObj.GetComponent<TextMeshProUGUI>();

            if (spaceToSkipText == null)
            {
                Debug.LogError("SpaceToSkip GameObject found, but no TextMeshProUGUI component attached!");
            }
        }
        else
        {
            Debug.LogError("SpaceToSkip text object not found in scene!");
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

    private void Update()
    {
        // Press Space to skip cutscene
        if (Input.GetKeyDown(KeyCode.Space) && !isSkipping)
        {
            Debug.Log("Space pressed! Fading to black and skipping...");
            StartCoroutine(FadeToBlackAndSkip());
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

    private IEnumerator FadeToBlackAndSkip()
    {
        isSkipping = true; // Prevent multiple skips
        fadePanel.gameObject.SetActive(true); // Show fade panel
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alphaValue = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadePanel.alpha = alphaValue; // Fade to black

            // Fade out "Press Space to Skip" text together (TextMeshPro version)
            if (spaceToSkipText != null)
            {
                Color textColor = spaceToSkipText.color;
                textColor.a = 1f - alphaValue; // Inverse fade (text disappears as screen fades to black)
                spaceToSkipText.color = textColor;
            }

            yield return null;
        }

        fadePanel.alpha = 1f; // Ensure it's fully black

        yield return new WaitForSeconds(skipDelay); //  Delay before loading the scene

        cutscenePlayer.Stop(); // Stop video
        LoadNextScene();
    }

    private void OnCutsceneEnd(VideoPlayer vp)
    {
        Debug.Log("Cutscene finished! Loading next scene...");
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(nextSceneName); // Directly load next scene
    }
}

