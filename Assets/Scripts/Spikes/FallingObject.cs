using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public string groundTag = "Ground"; // Ensure ground has this tag
    private bool hasHitGround = false; // Prevent multiple destruction calls

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasHitGround && collision.gameObject.CompareTag(groundTag))
        {
            hasHitGround = true;
            Destroy(gameObject, 1f); // Destroy after 1 sec to allow impact effect
        }
    }
}
