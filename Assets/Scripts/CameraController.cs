//using UnityEngine;

//public class CameraController : MonoBehaviour
//{
//    private Vector3 targetPoint = Vector3.zero;

//    public PlayerController player;

//    public float lookAheadDistance = 5f, lookAheadSpeed = 3f;

//    private float lookOffset;

//    private bool isFalling;
//    private bool wasGrounded; // Track the player's previous grounded state

//    public float maxVertOffset = 5f;

//    [Header("Camera Smoothing")]
//    public float normalLerpSpeed = 3f;      // Normal lerp speed during movement
//    public float landingLerpSpeed = 8f;     // Faster lerp speed when landing

//    private bool stopCameraMovement = false; // Flag to stop camera movement after death

//    public float verticalOffset = 2f; // Adjust this value to move the camera upward

//    private void Start()
//    {
//        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, -10);
//        SnapToPlayer(); // Instantly snap to the player's position at the start
//        wasGrounded = player.touchingDirections.IsGrounded; // Initialize grounded state

//        // Subscribe to the PlayerDied event
//        player.PlayerDied += OnPlayerDied;
//    }

//    private void LateUpdate()
//    {
//        // Stop camera movement if the player has died
//        if (stopCameraMovement)
//            return;

//        bool isGrounded = player.touchingDirections.IsGrounded;

//        // Check for landing event (transition from falling to grounded)
//        if (!wasGrounded && isGrounded)
//        {
//            targetPoint.y = player.transform.position.y + verticalOffset;
//        }

//        // Track vertical movement while falling
//        if (transform.position.y - player.transform.position.y > maxVertOffset)
//        {
//            isFalling = true;
//        }

//        if (isFalling)
//        {
//            targetPoint.y = player.transform.position.y + verticalOffset;

//            if (isGrounded)
//            {
//                isFalling = false;
//            }
//        }
//        else if (isGrounded)
//        {
//            targetPoint.y = player.transform.position.y + verticalOffset;
//        }

//        // Horizontal look-ahead logic
//        if (player.rb.linearVelocity.x > 0f)
//        {
//            lookOffset = Mathf.Lerp(lookOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime);
//        }
//        else if (player.rb.linearVelocity.x < 0f)
//        {
//            lookOffset = Mathf.Lerp(lookOffset, -lookAheadDistance, lookAheadSpeed * Time.deltaTime);
//        }

//        targetPoint.z = -10;
//        targetPoint.x = player.transform.position.x + lookOffset;

//        float lerpSpeed = (!wasGrounded && isGrounded) ? landingLerpSpeed : normalLerpSpeed;
//        transform.position = Vector3.Lerp(transform.position, targetPoint, lerpSpeed * Time.deltaTime);

//        wasGrounded = isGrounded;
//    }

//    private void SnapToPlayer()
//    {
//        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + verticalOffset, -10);
//    }

//    // Event handler to stop the camera when the player dies
//    private void OnPlayerDied()
//    {
//        stopCameraMovement = true;
//    }
//}


using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;

    [Header("Camera Zones")]
    public Transform[] cameraZones; // Array of fixed camera positions
    private int currentZoneIndex = 0; // Index of the current zone

    [Header("Camera Transition Settings")]
    public float transitionSpeed = 10f; // Speed of the camera movement
    private bool transitioning = false; // Flag to indicate if the camera is transitioning

    private void Start()
    {
        if (cameraZones.Length > 0)
        {
            transform.position = new Vector3(cameraZones[0].position.x, cameraZones[0].position.y, -10);
        }
    }

    private void LateUpdate()
    {
        if (!transitioning)
        {
            CheckZoneChange();
        }

        // Get target position from the current zone
        Vector3 targetPosition = new Vector3(
            cameraZones[currentZoneIndex].position.x,
            cameraZones[currentZoneIndex].position.y,
            -10f // Ensure camera stays in correct Z position
        );

        // Move towards target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, transitionSpeed * Time.deltaTime);

        // If camera reaches the target, stop transitioning
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition; // Snap to exact position
            transitioning = false;
            Debug.Log("Camera transition complete!");
        }
    }


    private void CheckZoneChange()
    {
        for (int i = 0; i < cameraZones.Length; i++)
        {
            if (IsPlayerInsideZone(cameraZones[i].position))
            {
                if (i != currentZoneIndex) // If player entered a new zone
                {
                    Debug.Log($"Transitioning to Zone {i}"); // Debug message
                    transitioning = true;
                    currentZoneIndex = i;
                    Invoke(nameof(FinishTransition), 0.5f); // Delay to smooth transition
                }
            }
        }
    }

    private bool IsPlayerInsideZone(Vector3 zonePosition)
    {
        float zoneWidth = 3f; // Increase detection width
        bool inside = player.transform.position.x > zonePosition.x - zoneWidth &&
                      player.transform.position.x < zonePosition.x + zoneWidth;

        if (inside)
        {
            Debug.Log("Player is inside the new camera zone.");
        }
        return inside;
    }

    private void FinishTransition()
    {
        transitioning = false;
    }
}

