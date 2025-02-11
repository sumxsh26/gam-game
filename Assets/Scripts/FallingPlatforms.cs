using System.Collections;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    //[SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private Rigidbody2D rb;

    [Header("Vibration Settings")]
    [SerializeField] private float shakeDuration = 0.5f; // Duration of the vibration before falling
    [SerializeField] private float shakeIntensity = 0.05f; // Intensity of the vibration

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

        // Apply gravity to the platform after vibration
        rb.bodyType = RigidbodyType2D.Dynamic;

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
}
