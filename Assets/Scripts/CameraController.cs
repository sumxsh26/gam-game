using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private Vector3 lastPosition;
    private bool allowMovement = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lastPosition = transform.position;
        Debug.Log("Camera start position locked at: " + lastPosition);
    }

    private void Update()
    {
        if (transform.position != lastPosition)
        {
            Debug.Log("Camera moved from: " + lastPosition + " to: " + transform.position);
            lastPosition = transform.position;
        }
    }

    public void SetCameraPosition(Vector3 newPosition)
    {
        if (!allowMovement) return;

        newPosition.z = -10f; // Always ensure correct Z position
        Debug.Log("Setting Camera Position to: " + newPosition);
        transform.position = newPosition;
        lastPosition = newPosition;
    }

    public void EnableCameraMovement()
    {
        allowMovement = true;
    }
}
