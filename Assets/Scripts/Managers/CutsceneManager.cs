using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro; // Import TextMeshPro namespace

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer cutscenePlayer; // Assign in Inspector
    public string nextSceneName = "Playtest level 1"; // Set next scene name
    public float skipDelay = 1.0f; // Delay before skipping

    private TextMeshProUGUI spaceToSkipText; // Updated to TextMeshProUGUI
    private bool isSkipping = false;

    private void Start()
    {
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
            cutscenePlayer.Play(); // Start cutscene immediately
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
            Debug.Log("Space pressed! Skipping cutscene...");
            StartCoroutine(SkipCutscene());
        }
    }

    private IEnumerator SkipCutscene()
    {
        isSkipping = true; // Prevent multiple skips
        yield return new WaitForSeconds(skipDelay); // Delay before skipping

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
        // Clean up persistent objects before transitioning
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

        foreach (var door in FindObjectsByType<DoorBehaviour>(FindObjectsSortMode.None))
        {
            Destroy(door.gameObject);
        }


        SceneManager.LoadSceneAsync(nextSceneName); // Load next scene
    }

}
