using UnityEngine;
using System.Collections;
using static UnityEditor.SceneView;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private Vector3 lastPosition;
    private bool allowMovement = false;
    private CameraZone startingZone; // Tracks the CameraZone to start in

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Find the CameraZone where the player starts
        startingZone = FindStartingZone();

        if (startingZone != null)
        {
            Vector3 zoneStartPosition = startingZone.transform.position;
            lastPosition = new Vector3(zoneStartPosition.x, zoneStartPosition.y, -10f);
            transform.position = lastPosition;

            Debug.Log($"[CAMERA INIT] Camera starting at CameraZone: {startingZone.name} at {lastPosition}");
        }
        else
        {
            Debug.LogWarning("[CAMERA INIT] No CameraZone found! Camera may not start correctly.");
        }
    }

    private void LateUpdate()
    {
        if (transform.position != lastPosition)
        {
            Debug.Log($"[CAMERA MOVEMENT] Camera moved from {lastPosition} to {transform.position}");
            lastPosition = transform.position;
        }
    }

    public void SetCameraPosition(Vector3 newPosition)
    {
        if (!allowMovement) return;

        Debug.Log($"[CAMERA DEBUG] Requested move to: {newPosition} | Current Position: {transform.position}");

        newPosition.z = -10f; // Ensure Z position stays fixed

        Debug.Log($"[CAMERA MOVEMENT] Camera moved from {transform.position} to {newPosition}");
        transform.position = newPosition;
        lastPosition = newPosition;
    }

    public void EnableCameraMovement()
    {
        allowMovement = true;
        Debug.Log("[CAMERA DEBUG] Camera movement enabled.");
    }

    private CameraZone FindStartingZone()
    {
        CameraZone[] zones = FindObjectsByType<CameraZone>(FindObjectsSortMode.None);
        foreach (CameraZone zone in zones)
        {
            Collider2D zoneCollider = zone.GetComponent<Collider2D>();
            if (zoneCollider != null && zoneCollider.OverlapPoint(Vector2.zero)) // Checks if the zone contains (0,0) or can be improved to check for the player position
            {
                return zone;
            }
        }

        return null;
    }
}
