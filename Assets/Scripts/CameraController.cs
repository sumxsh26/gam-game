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
//            targetPoint.y = player.transform.position.y;
//        }

//        // Track vertical movement while falling
//        if (transform.position.y - player.transform.position.y > maxVertOffset)
//        {
//            isFalling = true;
//        }

//        if (isFalling)
//        {
//            targetPoint.y = player.transform.position.y;

//            if (isGrounded)
//            {
//                isFalling = false;
//            }
//        }
//        else if (isGrounded)
//        {
//            targetPoint.y = player.transform.position.y;
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
//        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
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
    private Vector3 targetPoint = Vector3.zero;

    public PlayerController player;

    public float lookAheadDistance = 5f, lookAheadSpeed = 3f;

    private float lookOffset;

    private bool isFalling;
    private bool wasGrounded; // Track the player's previous grounded state

    public float maxVertOffset = 5f;

    [Header("Camera Smoothing")]
    public float normalLerpSpeed = 3f;      // Normal lerp speed during movement
    public float landingLerpSpeed = 8f;     // Faster lerp speed when landing

    private bool stopCameraMovement = false; // Flag to stop camera movement after death

    public float verticalOffset = 2f; // Adjust this value to move the camera upward

    private void Start()
    {
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        SnapToPlayer(); // Instantly snap to the player's position at the start
        wasGrounded = player.touchingDirections.IsGrounded; // Initialize grounded state

        // Subscribe to the PlayerDied event
        player.PlayerDied += OnPlayerDied;
    }

    private void LateUpdate()
    {
        // Stop camera movement if the player has died
        if (stopCameraMovement)
            return;

        bool isGrounded = player.touchingDirections.IsGrounded;

        // Check for landing event (transition from falling to grounded)
        if (!wasGrounded && isGrounded)
        {
            targetPoint.y = player.transform.position.y + verticalOffset;
        }

        // Track vertical movement while falling
        if (transform.position.y - player.transform.position.y > maxVertOffset)
        {
            isFalling = true;
        }

        if (isFalling)
        {
            targetPoint.y = player.transform.position.y + verticalOffset;

            if (isGrounded)
            {
                isFalling = false;
            }
        }
        else if (isGrounded)
        {
            targetPoint.y = player.transform.position.y + verticalOffset;
        }

        // Horizontal look-ahead logic
        if (player.rb.linearVelocity.x > 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }
        else if (player.rb.linearVelocity.x < 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, -lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        }

        targetPoint.z = -10;
        targetPoint.x = player.transform.position.x + lookOffset;

        float lerpSpeed = (!wasGrounded && isGrounded) ? landingLerpSpeed : normalLerpSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPoint, lerpSpeed * Time.deltaTime);

        wasGrounded = isGrounded;
    }

    private void SnapToPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + verticalOffset, -10);
    }

    // Event handler to stop the camera when the player dies
    private void OnPlayerDied()
    {
        stopCameraMovement = true;
    }
}
