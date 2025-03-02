//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CompleteZone : MonoBehaviour
//{
//    [SerializeField] private string nextSceneName; // Set in the Inspector

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            Debug.Log($"Loading scene: {nextSceneName}");
//            SceneManager.LoadScene(nextSceneName);
//        }
//    }
//}

//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CompleteZone : MonoBehaviour
//{
//    [SerializeField] private string nextSceneName; // Set in the Inspector
//    [SerializeField] private float fadeDuration = 1.5f; // Time to fade out the player before switching scenes

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            Debug.Log($"Player entered Complete Zone. Fading out before loading scene: {nextSceneName}");

//            // Get the player's sprite renderer and start fading
//            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
//            if (playerSprite != null)
//            {
//                StartCoroutine(FadeOutAndLoadScene(playerSprite));
//            }
//            else
//            {
//                // If the player has no sprite renderer, load scene immediately
//                Debug.LogWarning("No SpriteRenderer found on Player. Loading scene immediately.");
//                SceneManager.LoadScene(nextSceneName);
//            }
//        }
//    }

//    private IEnumerator FadeOutAndLoadScene(SpriteRenderer spriteRenderer)
//    {
//        float elapsedTime = 0f;
//        Color originalColor = spriteRenderer.color;

//        while (elapsedTime < fadeDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
//            yield return null;
//        }

//        // Ensure the player is fully faded out
//        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

//        // Wait a short moment before loading the next scene
//        yield return new WaitForSeconds(0.5f);

//        Debug.Log($"Fading complete. Loading scene: {nextSceneName}");
//        SceneManager.LoadScene(nextSceneName);
//    }
//}


//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CompleteZone : MonoBehaviour
//{
//    [SerializeField] private string nextSceneName; // Set in the Inspector
//    [SerializeField] private float fadeDuration = 0.5f; // Time to fade out the player before switching scenes
//    private bool isTransitioning = false; // Prevent multiple triggers

//    //private void OnTriggerEnter2D(Collider2D collision)
//    //{
//    //    if (collision.CompareTag("Player") && !isTransitioning)
//    //    {
//    //        isTransitioning = true; // Prevent retriggering
//    //        Debug.Log($"Player entered Complete Zone. Fading out before loading scene: {nextSceneName}");

//    //        // Force Unity to update collision detection immediately
//    //        Physics2D.SyncTransforms();

//    //        // Get the player's sprite renderer and start fading
//    //        SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
//    //        if (playerSprite != null)
//    //        {
//    //            StartCoroutine(FadeOutAndLoadScene(playerSprite));
//    //        }
//    //        else
//    //        {
//    //            Debug.LogWarning("No SpriteRenderer found on Player. Loading scene immediately.");
//    //            SceneManager.LoadScene(nextSceneName);
//    //        }
//    //    }
//    //}

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && !isTransitioning)
//        {
//            isTransitioning = true; // Prevent retriggering
//            Debug.Log("[DEBUG] Player detected in Complete Zone. Starting transition.");

//            Physics2D.SyncTransforms(); // Ensure physics updates instantly

//            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
//            if (playerSprite != null)
//            {
//                Debug.Log("[DEBUG] Player sprite found. Starting fade out immediately.");
//                FadeOutAndLoadScene_Immediate(playerSprite);
//            }
//            else
//            {
//                Debug.LogWarning("[DEBUG] No SpriteRenderer found on Player. Loading scene immediately.");
//                SceneManager.LoadScene(nextSceneName);
//            }
//        }
//    }


//    private void FadeOutAndLoadScene_Immediate(SpriteRenderer spriteRenderer)
//    {
//        Debug.Log("[DEBUG] FadeOutAndLoadScene started immediately.");

//        StartCoroutine(FadeOutAndLoadScene(spriteRenderer));
//    }

//    private IEnumerator FadeOutAndLoadScene(SpriteRenderer spriteRenderer)
//    {
//        float elapsedTime = 0f;
//        Color originalColor = spriteRenderer.color;

//        while (elapsedTime < fadeDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
//            yield return null;
//        }

//        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
//        Debug.Log("[DEBUG] Player fully faded out. Loading scene now.");
//        SceneManager.LoadScene(nextSceneName);
//    }



//    //private IEnumerator FadeOutAndLoadScene(SpriteRenderer spriteRenderer)
//    //{
//    //    float elapsedTime = 0f;
//    //    Color originalColor = spriteRenderer.color;

//    //    while (elapsedTime < fadeDuration)
//    //    {
//    //        elapsedTime += Time.deltaTime;
//    //        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//    //        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
//    //        yield return null;
//    //    }

//    //    spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
//    //    //yield return new WaitForSeconds(0.2f);

//    //    Debug.Log($"Fading complete. Loading scene: {nextSceneName}");
//    //    SceneManager.LoadScene(nextSceneName);
//    //}
//}


//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class CompleteZone : MonoBehaviour
//{
//    [SerializeField] private string nextSceneName;
//    [SerializeField] private float fadeDuration = 0.5f;

//    private bool isTransitioning = false;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && !isTransitioning)
//        {
//            isTransitioning = true;
//            Debug.Log("[DEBUG] Player entered Complete Zone. Starting fade-out.");

//            // Get the player's sprite renderer and start fading
//            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
//            if (playerSprite != null)
//            {
//                StartCoroutine(FadeOutAndLoadScene(playerSprite));
//            }
//            else
//            {
//                Debug.LogWarning("[DEBUG] No SpriteRenderer found on Player. Loading scene immediately.");
//                SceneManager.LoadScene(nextSceneName);
//            }
//        }
//    }

//    private IEnumerator FadeOutAndLoadScene(SpriteRenderer spriteRenderer)
//    {
//        Debug.Log("[DEBUG] FadeOutAndLoadScene started.");

//        float elapsedTime = 0f;
//        Color originalColor = spriteRenderer.color;

//        while (elapsedTime < fadeDuration)
//        {
//            elapsedTime += Time.deltaTime;
//            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
//            yield return null;
//        }

//        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
//        Debug.Log("[DEBUG] Player fully faded out. Loading scene now.");

//        SceneManager.LoadScene(nextSceneName);
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteZone : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private float fadeDuration = 0.3f; // Adjust this for faster fading

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            Debug.Log("[DEBUG] Player entered Complete Zone. Starting immediate fade-out.");

            // Immediately start fade-out, ignoring SlidingDoor state
            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                Debug.Log("[DEBUG] Player sprite found. Starting fade now.");
                StartCoroutine(FadeOutAndLoadScene(playerSprite));
            }
            else
            {
                Debug.LogWarning("[DEBUG] No SpriteRenderer found on Player. Loading scene immediately.");
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private IEnumerator FadeOutAndLoadScene(SpriteRenderer spriteRenderer)
    {
        Debug.Log("[DEBUG] FadeOutAndLoadScene coroutine started immediately.");

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Debug.Log("[DEBUG] Player fully faded out. Loading scene now.");

        SceneManager.LoadScene(nextSceneName);
    }
}
