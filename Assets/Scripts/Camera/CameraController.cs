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
        }
    }

    private void LateUpdate()
    {
        if (transform.position != lastPosition)
        {
            lastPosition = transform.position;
        }
    }

    public void SetCameraPosition(Vector3 newPosition)
    {
        if (!allowMovement) return;

        newPosition.z = -10f; // Ensure Z position stays fixed

        transform.position = newPosition;
        lastPosition = newPosition;
    }

    public void EnableCameraMovement()
    {
        allowMovement = true;
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
