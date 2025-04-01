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
