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


using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteZone : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Set in the Inspector
    [SerializeField] private float fadeDuration = 1.5f; // Time to fade out the player before switching scenes
    private bool isTransitioning = false; // Prevent multiple triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true; // Prevent retriggering
            Debug.Log($"Player entered Complete Zone. Fading out before loading scene: {nextSceneName}");

            // Get the player's sprite renderer and start fading
            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                StartCoroutine(FadeOutAndLoadScene(playerSprite));
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on Player. Loading scene immediately.");
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private IEnumerator FadeOutAndLoadScene(SpriteRenderer spriteRenderer)
    {
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
        yield return new WaitForSeconds(0.5f);

        Debug.Log($"Fading complete. Loading scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }
}
