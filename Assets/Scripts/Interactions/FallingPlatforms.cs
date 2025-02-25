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

using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatforms : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D platformCollider; // Reference to the platform's collider
    [SerializeField] private Tilemap tilemap; // Reference to the tilemap

    [Header("Vibration Settings")]
    [SerializeField] private float shakeDuration = 0.5f; // Duration of the vibration before falling
    [SerializeField] private float shakeIntensity = 0.05f; // Intensity of the vibration

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f; // Duration of the fade effect

    private Vector3 originalPosition; // To reset after shaking

    private void Start()
    {
        originalPosition = transform.localPosition; // Store the initial position
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
        // Vibrate before falling
        yield return StartCoroutine(Vibrate(shakeDuration, shakeIntensity));

        // Disable the collider to prevent the player from standing on it
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        // Apply gravity to the platform after vibration
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Start fading the tilemap
        StartCoroutine(FadeOut());

        // Destroy the platform after a delay
        Destroy(gameObject, destroyDelay);
    }

    private IEnumerator Vibrate(float duration, float intensity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Apply a small random offset to create a shaking effect
            Vector3 randomOffset = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
            transform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the platform to its original position after shaking
        transform.localPosition = originalPosition;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color originalColor = tilemap.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully invisible at the end
        tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}

