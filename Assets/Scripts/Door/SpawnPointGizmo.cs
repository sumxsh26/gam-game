using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    public GameObject doorPrefab; // Assign the Door prefab in the Inspector

    private void OnDrawGizmos()
    {
        if (doorPrefab != null)
        {
            // Get all SpriteRenderers inside the doorPrefab
            SpriteRenderer[] sprites = doorPrefab.GetComponentsInChildren<SpriteRenderer>();

            if (sprites.Length > 0)
            {
                Bounds totalBounds = sprites[0].bounds;

                // Expand the bounds to include all sprites
                foreach (SpriteRenderer sprite in sprites)
                {
                    totalBounds.Encapsulate(sprite.bounds);
                }

                Vector3 size = totalBounds.size; // Get the combined size
                Vector3 centerOffset = totalBounds.center - doorPrefab.transform.position; // Calculate offset

                Gizmos.color = Color.green; // Set outline color
                Gizmos.DrawWireCube(transform.position + centerOffset, size); // Adjust position to align correctly
            }
            else
            {
                Debug.LogWarning("No SpriteRenderers found inside DoorPrefab! Using default size.");
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position, new Vector3(2, 3, 0)); // Default approximate size
            }
        }
    }
}
