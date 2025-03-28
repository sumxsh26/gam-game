using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatformTracker : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private Tilemap tilemap;

    private Color originalColor;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
        tilemap = GetComponent<Tilemap>();

        if (tilemap != null)
        {
            originalColor = tilemap.color;
        }

        FallingPlatformManager.Instance?.RegisterPlatform(this);
    }

    public void ResetPlatform()
    {
        // Reset position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Reset Rigidbody
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Re-enable collider
        if (platformCollider != null)
        {
            platformCollider.enabled = true;
        }

        // Reset fade
        if (tilemap != null)
        {
            tilemap.color = originalColor;
        }

        gameObject.SetActive(true);
    }
}
