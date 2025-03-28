//using System.Collections;
//using UnityEngine;

//public class FallingPlatforms : MonoBehaviour
//{
//    //[SerializeField] private float fallDelay = 1f;
//    [SerializeField] private float destroyDelay = 2f;
//    [SerializeField] private Rigidbody2D rb;

//    [Header("Vibration Settings")]
//    [SerializeField] private float shakeDuration = 0.5f; // Duration of the vibration before falling
//    [SerializeField] private float shakeIntensity = 0.05f; // Intensity of the vibration

//    private Vector3 originalPosition; // To reset after shaking

//    private void Start()
//    {
//        originalPosition = transform.localPosition; // Store the initial position
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            StartCoroutine(Fall());
//        }
//    }

//    private IEnumerator Fall()
//    {
//        // Vibrate before falling
//        yield return StartCoroutine(Vibrate(shakeDuration, shakeIntensity));

//        // Apply gravity to the platform after vibration
//        rb.bodyType = RigidbodyType2D.Dynamic;

//        // Destroy the platform after a delay
//        Destroy(gameObject, destroyDelay);
//    }

//    private IEnumerator Vibrate(float duration, float intensity)
//    {
//        float elapsedTime = 0f;

//        while (elapsedTime < duration)
//        {
//            // Apply a small random offset to create a shaking effect
//            Vector3 randomOffset = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
//            transform.localPosition = originalPosition + randomOffset;

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        // Reset the platform to its original position after shaking
//        transform.localPosition = originalPosition;
//    }
//}

//using System.Collections;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class FallingPlatforms : MonoBehaviour
//{
//    [SerializeField] private float destroyDelay = 2f;
//    [SerializeField] private Rigidbody2D rb;
//    [SerializeField] private Collider2D platformCollider; // Reference to the platform's collider
//    [SerializeField] private Tilemap tilemap; // Reference to the tilemap

//    [Header("Vibration Settings")]
//    [SerializeField] private float shakeDuration = 0.5f; // Duration of the vibration before falling
//    [SerializeField] private float shakeIntensity = 0.05f; // Intensity of the vibration

//    [Header("Fade Settings")]
//    [SerializeField] private float fadeDuration = 1.5f; // Duration of the fade effect

//    private Vector3 originalPosition; // To reset after shaking

//    private void Start()
//    {
//        originalPosition = transform.localPosition; // Store the initial position
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            StartCoroutine(Fall());
//        }
//    }

//    private IEnumerator Fall()
//    {
//        // Vibrate before falling
//        yield return StartCoroutine(Vibrate(shakeDuration, shakeIntensity));

//        // Disable the collider to prevent the player from standing on it
//        if (platformCollider != null)
//        {
//            platformCollider.enabled = false;
//        }

//        // Apply gravity to the platform after vibration
//        rb.bodyType = RigidbodyType2D.Dynamic;

//        // Start fading the tilemap
//        StartCoroutine(FadeOut());

//        // Destroy the platform after a delay
//        Destroy(gameObject, destroyDelay);
//    }

//    private IEnumerator Vibrate(float duration, float intensity)
//    {
//        float elapsedTime = 0f;

//        while (elapsedTime < duration)
//        {
//            // Apply a small random offset to create a shaking effect
//            Vector3 randomOffset = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
//            transform.localPosition = originalPosition + randomOffset;

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        // Reset the platform to its original position after shaking
//        transform.localPosition = originalPosition;
//    }

//    private IEnumerator FadeOut()
//    {
//        float elapsedTime = 0f;
//        Color originalColor = tilemap.color;

//        while (elapsedTime < fadeDuration)
//        {
//            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        // Ensure it's fully invisible at the end
//        tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
//    }
//}

//using System.Collections;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class FallingPlatforms : MonoBehaviour
//{
//    [SerializeField] private float destroyDelay = 2f;
//    [SerializeField] private Rigidbody2D rb;
//    [SerializeField] private Collider2D platformCollider;
//    [SerializeField] private Tilemap tilemap;

//    [Header("Vibration Settings")]
//    [SerializeField] private float shakeDuration = 1f; // Longer shake before fall
//    [SerializeField] private float startShakeIntensity = 0.02f;
//    [SerializeField] private float endShakeIntensity = 0.1f;

//    [Header("Fade Settings")]
//    [SerializeField] private float fadeDuration = 1.5f;

//    private Vector3 originalPosition;

//    private void Start()
//    {
//        originalPosition = transform.localPosition;
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            StartCoroutine(Fall());
//        }
//    }

//    private IEnumerator Fall()
//    {
//        yield return StartCoroutine(Vibrate(shakeDuration));

//        if (platformCollider != null)
//        {
//            platformCollider.enabled = false;
//        }

//        rb.bodyType = RigidbodyType2D.Dynamic;
//        rb.gravityScale = 10f; // Fall faster

//        StartCoroutine(FadeOut());

//        Destroy(gameObject, destroyDelay);
//    }

//    private IEnumerator Vibrate(float duration)
//    {
//        float elapsedTime = 0f;

//        while (elapsedTime < duration)
//        {
//            float t = elapsedTime / duration;
//            float currentIntensity = Mathf.Lerp(startShakeIntensity, endShakeIntensity, t);

//            Vector3 randomOffset = new Vector3(
//                Random.Range(-currentIntensity, currentIntensity),
//                Random.Range(-currentIntensity, currentIntensity),
//                0f
//            );

//            transform.localPosition = originalPosition + randomOffset;

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        transform.localPosition = originalPosition;
//    }

//    private IEnumerator FadeOut()
//    {
//        float elapsedTime = 0f;
//        Color originalColor = tilemap.color;

//        while (elapsedTime < fadeDuration)
//        {
//            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
//            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
//    }
//}


using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatforms : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private Tilemap tilemap;

    [Header("Vibration Settings")]
    [SerializeField] private float shakeDuration = 1.2f;          // Total time before falling
    [SerializeField] private float initialShakeIntensity = 0.02f; // Start soft
    [SerializeField] private float finalShakeIntensity = 0.1f;    // Get stronger

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        // Shake first
        yield return StartCoroutine(Vibrate(shakeDuration));

        // Disable collider before fall
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        // Enable physics to fall quickly
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 10f;

        // Start fading
        StartCoroutine(FadeOut());

        // Hide after delay
        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }

    private IEnumerator Vibrate(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float intensity = Mathf.Lerp(initialShakeIntensity, finalShakeIntensity, t);

            Vector3 offset = new Vector3(
                Random.Range(-intensity, intensity),
                Random.Range(-intensity, intensity),
                0f
            );

            transform.localPosition = originalPosition + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color originalColor = tilemap.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
