using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CameraZone : MonoBehaviour
{
    private static CameraZone currentZone; // Tracks the active zone
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        AdjustColliderSize();
    }

    private void OnValidate()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        AdjustColliderSize();
    }

    private void AdjustColliderSize()
    {
        if (Camera.main == null || boxCollider == null) return;

        float camHeight = 2f * Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        boxCollider.size = new Vector2(camWidth, camHeight);
        boxCollider.offset = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null) return;

            // Find the best zone for the player
            CameraZone bestZone = FindBestZoneForPlayer(player);

            if (bestZone != null && bestZone != currentZone)
            {
                Debug.Log($"[CAMERA DEBUG] Switching Camera to new zone: {bestZone.name}");
                currentZone = bestZone;
                currentZone.ActivateZone(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentZone == this)
        {
            Debug.Log($"[CAMERA DEBUG] Player exited {name}, checking for best zone...");
            CameraZone bestZone = FindBestZoneForPlayer(other.GetComponent<PlayerController>());

            if (bestZone != null && bestZone != this)
            {
                Debug.Log($"[CAMERA DEBUG] Switching Camera to new detected zone: {bestZone.name}");
                currentZone = bestZone;
                currentZone.ActivateZone(other.GetComponent<PlayerController>());
            }
        }
    }

    private void ActivateZone(PlayerController player)
    {
        CameraController.instance.EnableCameraMovement();
        StartCoroutine(DelayedCameraMove(player));
    }

    private IEnumerator DelayedCameraMove(PlayerController player)
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 newCameraPosition = transform.position;
        newCameraPosition.z = -10f; // Force correct Z position

        Debug.Log($"[CAMERA DEBUG] Player Position: {player.transform.position}");
        Debug.Log($"[CAMERA DEBUG] Moving Camera to Zone: {name} at Position: {transform.position} (Forcing Z = -10)");

        CameraController.instance.SetCameraPosition(newCameraPosition);
    }

    private CameraZone FindBestZoneForPlayer(PlayerController player)
    {
        if (player == null) return this; // Default to current zone

        List<CameraZone> overlappingZones = new List<CameraZone>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(player.transform.position, Vector2.one, 0f);

        foreach (Collider2D col in colliders)
        {
            CameraZone zone = col.GetComponent<CameraZone>();
            if (zone != null)
            {
                overlappingZones.Add(zone);
            }
        }

        if (overlappingZones.Count == 1)
        {
            return overlappingZones[0]; // If only one zone detected, return it
        }
        else if (overlappingZones.Count > 1)
        {
            // Find the zone the player is MOST inside
            CameraZone bestZone = null;
            float maxOverlap = 0f;

            foreach (CameraZone zone in overlappingZones)
            {
                float overlapAmount = GetOverlapPercentage(player, zone);
                if (overlapAmount > maxOverlap)
                {
                    maxOverlap = overlapAmount;
                    bestZone = zone;
                }
            }

            return bestZone ?? this; // Return the best match or default
        }

        return this; // Default to current zone if no better one found
    }

    private float GetOverlapPercentage(PlayerController player, CameraZone zone)
    {
        if (player == null || zone == null) return 0f;

        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        if (playerCollider == null) return 0f;

        Bounds playerBounds = playerCollider.bounds;
        Bounds zoneBounds = zone.boxCollider.bounds;

        float overlapWidth = Mathf.Min(playerBounds.max.x, zoneBounds.max.x) - Mathf.Max(playerBounds.min.x, zoneBounds.min.x);
        float overlapHeight = Mathf.Min(playerBounds.max.y, zoneBounds.max.y) - Mathf.Max(playerBounds.min.y, zoneBounds.min.y);

        if (overlapWidth < 0 || overlapHeight < 0) return 0f; // No overlap

        float playerArea = playerBounds.size.x * playerBounds.size.y;
        float overlapArea = overlapWidth * overlapHeight;

        return overlapArea / playerArea; // Percentage of player inside zone
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            Vector3 colliderCenter = (Vector3)boxCollider.offset + transform.position;
            Gizmos.DrawWireCube(colliderCenter, boxCollider.size);
        }
    }
}
