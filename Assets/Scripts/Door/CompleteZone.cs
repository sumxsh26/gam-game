//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CompleteZone : MonoBehaviour
//{
//    [SerializeField] private string nextSceneName;  // Name of the next level
//    [SerializeField] private float fadeDuration = 0.3f; // Adjust for faster fading
//    private bool isTransitioning = false;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && !isTransitioning)
//        {
//            isTransitioning = true;
//            Debug.Log("[DEBUG] Player entered Complete Zone. Starting fade-out.");

//            // Store the next level name in PlayerPrefs
//            PlayerPrefs.SetString("NextLevel", nextSceneName);
//            PlayerPrefs.Save(); // Ensure it's stored before switching scenes

//            // Get the player's sprite
//            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();

//            if (playerSprite != null)
//            {
//                StartCoroutine(FadeOutAndLoadLoadingScene(playerSprite));
//            }
//            else
//            {
//                Debug.LogWarning("[DEBUG] No SpriteRenderer found on Player. Loading loading scene immediately.");
//                SceneManager.LoadScene("Loading");
//            }
//        }
//    }

//    private IEnumerator FadeOutAndLoadLoadingScene(SpriteRenderer playerSprite)
//    {
//        Debug.Log("[DEBUG] FadeOutAndLoadLoadingScene coroutine started.");

//        float elapsedTime = 0f;
//        Color playerOriginalColor = playerSprite.color;

//        // Detect carried mouse via child Mice component
//        Mice carriedMouse = playerSprite.GetComponentInChildren<Mice>();
//        SpriteRenderer mouseSprite = carriedMouse != null ? carriedMouse.GetComponent<SpriteRenderer>() : null;
//        Color mouseOriginalColor = mouseSprite != null ? mouseSprite.color : Color.white;

//        // Start fading
//        while (elapsedTime < fadeDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

//            // Fade out player
//            playerSprite.color = new Color(playerOriginalColor.r, playerOriginalColor.g, playerOriginalColor.b, alpha);

//            // Fade out mouse if carried
//            if (mouseSprite != null)
//            {
//                mouseSprite.color = new Color(mouseOriginalColor.r, mouseOriginalColor.g, mouseOriginalColor.b, alpha);
//            }

//            yield return null;
//        }

//        // Ensure fully transparent at end
//        playerSprite.color = new Color(playerOriginalColor.r, playerOriginalColor.g, playerOriginalColor.b, 0f);
//        if (mouseSprite != null)
//        {
//            mouseSprite.color = new Color(mouseOriginalColor.r, mouseOriginalColor.g, mouseOriginalColor.b, 0f);
//        }

//        // Fade and destroy carried mouse (optional)
//        if (carriedMouse != null)
//        {
//            carriedMouse.FadeAndDestroy();
//        }

//        // Clean up persistent managers
//        if (CheckpointManager.Instance != null)
//        {
//            CheckpointManager.Instance.DestroyCheckpointsOnLevelTransition();
//        }

//        if (FallingSpikeManager.Instance != null)
//        {
//            FallingSpikeManager.Instance.DestroySpikesOnLevelTransition();
//        }

//        if (FallingPlatformManager.Instance != null)
//        {
//            FallingPlatformManager.Instance.DestroyPlatformsOnLevelTransition();
//        }

//        Debug.Log("[DEBUG] Player and mouse faded out. Loading loading scene.");
//        SceneManager.LoadScene("Loading");
//    }

//}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteZone : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName;         // Name of the next level
    [SerializeField] private bool useLoadingScreen = true; // Toggle this in Inspector
    [SerializeField] private float fadeDuration = 0.3f;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            Debug.Log("[DEBUG] Player entered Complete Zone. Starting fade-out.");

            // Store the next level name in PlayerPrefs (used by loading screen if needed)
            PlayerPrefs.SetString("NextLevel", nextSceneName);
            PlayerPrefs.Save();

            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();

            if (playerSprite != null)
            {
                StartCoroutine(FadeOutAndLoadNext(playerSprite));
            }
            else
            {
                Debug.LogWarning("[DEBUG] No SpriteRenderer found on Player. Skipping fade.");
                LoadSceneNow(); // Skip fade if player sprite is missing
            }
        }
    }

    private IEnumerator FadeOutAndLoadNext(SpriteRenderer playerSprite)
    {
        Debug.Log("[DEBUG] FadeOutAndLoadNext coroutine started.");

        float elapsedTime = 0f;
        Color playerOriginalColor = playerSprite.color;

        // Fade out carried mouse too
        Mice carriedMouse = playerSprite.GetComponentInChildren<Mice>();
        SpriteRenderer mouseSprite = carriedMouse != null ? carriedMouse.GetComponent<SpriteRenderer>() : null;
        Color mouseOriginalColor = mouseSprite != null ? mouseSprite.color : Color.white;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            playerSprite.color = new Color(playerOriginalColor.r, playerOriginalColor.g, playerOriginalColor.b, alpha);

            if (mouseSprite != null)
            {
                mouseSprite.color = new Color(mouseOriginalColor.r, mouseOriginalColor.g, mouseOriginalColor.b, alpha);
            }

            yield return null;
        }

        playerSprite.color = new Color(playerOriginalColor.r, playerOriginalColor.g, playerOriginalColor.b, 0f);
        if (mouseSprite != null)
        {
            mouseSprite.color = new Color(mouseOriginalColor.r, mouseOriginalColor.g, mouseOriginalColor.b, 0f);
        }

        if (carriedMouse != null)
        {
            carriedMouse.FadeAndDestroy();
        }

        // Clean up persistent managers
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.DestroyCheckpointsOnLevelTransition();

        if (FallingSpikeManager.Instance != null)
            FallingSpikeManager.Instance.DestroySpikesOnLevelTransition();

        if (FallingPlatformManager.Instance != null)
            FallingPlatformManager.Instance.DestroyPlatformsOnLevelTransition();

        foreach (var door in FindObjectsByType<DoorBehaviour>(FindObjectsSortMode.None))
        {
            Destroy(door.gameObject);
        }


        Debug.Log("[DEBUG] Fade complete. Deciding next scene...");

        LoadSceneNow();
    }

    private void LoadSceneNow()
    {
        if (useLoadingScreen)
        {
            Debug.Log("[DEBUG] Loading 'Loading' scene...");
            SceneManager.LoadScene("Loading"); // Loading screen uses PlayerPrefs["NextLevel"]
        }
        else
        {
            Debug.Log("[DEBUG] Directly loading: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
