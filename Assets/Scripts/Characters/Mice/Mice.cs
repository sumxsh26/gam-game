//using UnityEngine;

//public class Mice : MonoBehaviour
//{
//    [SerializeField] GameObject player;
//    public bool isPickedUp;
//    private Vector2 vel;
//    public float smoothTime;

//    // Start
//    void Start()
//    {
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isPickedUp)
//        {
//            Vector3 offset = new Vector3(0, 1.7f, 0);
//            transform.position = Vector2.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.gameObject.CompareTag("Player") && !isPickedUp)
//        {
//            isPickedUp = true;
//        }
//    }
//}



//// update - linking to toggling and mouse AI
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
//public class Mice : MonoBehaviour
//{
//    public float walkSpeed = 3f;
//    public float escapeSpeed = 6f; // Speed when escaping from the player
//    public float walkStopRate = 0.05f; // Smooth deceleration
//    public DetectionZone playerDetectionZone; // Detects the player to run away
//    public DetectionZone cliffDetectionZone; // Detects ledges

//    private Rigidbody2D rb;
//    private TouchingDirections touchingDirections;
//    private Animator animator;
//    private Vector2 walkDirectionVector = Vector2.right;
//    private float flipCooldown = 0.2f; // Delay to prevent jittering
//    private float lastFlipTime = 0f;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        touchingDirections = GetComponent<TouchingDirections>();
//        animator = GetComponent<Animator>();
//    }

//    private void FixedUpdate()
//    {
//        MoveMouse();
//    }

//    private void MoveMouse()
//    {
//        if (touchingDirections == null || cliffDetectionZone == null)
//            return;

//        bool isBlockedByWall = touchingDirections.IsGrounded && touchingDirections.IsOnWall;
//        bool isAtLedge = cliffDetectionZone.detectedColliders.Count == 0;

//        // Flip if the mouse hits a wall or reaches a ledge
//        if ((isBlockedByWall || isAtLedge) && Time.time - lastFlipTime > flipCooldown)
//        {
//            FlipDirection();
//        }

//        float currentSpeed = walkSpeed;

//        // If the player is nearby, make the mouse run away faster AND flip direction
//        if (playerDetectionZone.detectedColliders.Count > 0)
//        {
//            currentSpeed = escapeSpeed;
//            FlipDirection(); // Flip to run away from the player
//        }

//        rb.linearVelocity = new Vector2(currentSpeed * walkDirectionVector.x, rb.linearVelocity.y);
//    }

//    private void FlipDirection()
//    {
//        if (Time.time - lastFlipTime < flipCooldown) return;

//        // Flip movement direction
//        walkDirectionVector = -walkDirectionVector;
//        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

//        lastFlipTime = Time.time; // Reset flip cooldown
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("Player detected! Picking up mouse.");

//            // Toggle platforms upon pickup
//            TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
//            foreach (TilemapToggle toggle in toggles)
//            {
//                toggle.TogglePlatform();
//            }

//            Destroy(gameObject); // Destroy the mouse after pickup
//        }
//    }
//}

// blue and red mice pickup
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Mice : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float escapeSpeed = 6f;
    public float walkStopRate = 0.05f;
    public DetectionZone playerDetectionZone;
    public DetectionZone cliffDetectionZone;
    public bool isBlueMouse;

    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Vector2 walkDirectionVector = Vector2.right;
    private float flipCooldown = 0.2f;
    private float lastFlipTime = 0f;
    private bool isFollowingPlayer = false;
    private Transform player;
    private Vector3 initialPickupPosition;

    // distance from player
    private Vector3 followOffset = new Vector3(-1.5f, 0, 0); // Distance behind the player
    private float followSpeed = 5f; // Speed of following
    //private float minFollowDistance = 1.2f; // Minimum distance before mouse moves closer

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        initialPickupPosition = transform.position;
    }

    private bool isPickedUp = false; // Track if the mouse has been picked up

    private void FixedUpdate()
    {
        if (!isFollowingPlayer)
        {
            MoveMouse();
        }
        else
        {
            FollowPlayer();
        }

        // Update animation state ONLY if picked up
        if (isPickedUp)
        {
            UpdateAnimationState();
        }
        else
        {
            // Before pickup, always stay in moving animation
            animator.SetBool(AnimationStrings.isMoving, true);
        }
    }

    private void UpdateAnimationState()
    {
        if (animator != null)
        {
            // Mouse only switches between moving and idle if picked up
            bool isMoving = isFollowingPlayer && Mathf.Abs(InputManager.Movement.x) > 0;
            animator.SetBool(AnimationStrings.isMoving, isMoving);
        }
    }

    // Called when player picks up the mouse
    //public void SetFollowingPlayer(Transform playerTransform)
    //{
    //    player = playerTransform;
    //    isFollowingPlayer = true;
    //    isPickedUp = true; // Mark mouse as picked up
    //    FlipToMatchPlayer();
    //}

    public void SetFollowingPlayer(Transform playerTransform)
    {
        player = playerTransform;
        isFollowingPlayer = true;
        isPickedUp = true; // Mark as picked up

        // Disable the BoxCollider2D inside PickupZone
        Transform pickupZone = transform.Find("PickupZone");
        if (pickupZone != null)
        {
            BoxCollider2D collider = pickupZone.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }



    private void MoveMouse()
    {
        if (touchingDirections == null || cliffDetectionZone == null)
            return;

        bool isBlockedByWall = touchingDirections.IsGrounded && touchingDirections.IsOnWall;
        bool isAtLedge = cliffDetectionZone.detectedColliders.Count == 0;

        if ((isBlockedByWall || isAtLedge) && Time.time - lastFlipTime > flipCooldown)
        {
            FlipDirection();
        }

        float currentSpeed = walkSpeed;

        if (playerDetectionZone.detectedColliders.Count > 0)
        {
            currentSpeed = escapeSpeed;
            FlipDirection();
        }

        rb.linearVelocity = new Vector2(currentSpeed * walkDirectionVector.x, rb.linearVelocity.y);
    }
    private void FollowPlayer()
    {
        if (player != null)
        {
            // Get player's facing direction
            float playerDirection = Mathf.Sign(player.localScale.x);

            // Always follow behind the player
            Vector3 targetPosition = player.position + new Vector3(followOffset.x * playerDirection, followOffset.y, 0);

            // Smoothly move the mouse to the target position
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

            // Flip the mouse to always face the same direction as the player
            FlipToMatchPlayer(playerDirection);
        }
    }

    // Flip the mouse to match player's facing direction
    private void FlipToMatchPlayer(float playerDirection)
    {
        transform.localScale = new Vector3(playerDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


    private void FlipDirection()
    {
        if (Time.time - lastFlipTime < flipCooldown) return;

        walkDirectionVector = -walkDirectionVector;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        lastFlipTime = Time.time;
    }

    //public void DropMouse()
    //{
    //    isFollowingPlayer = false;
    //    transform.position = initialPickupPosition;
    //}

    //public void DropMouse()
    //{
    //    isFollowingPlayer = false;
    //    isPickedUp = false;

    //    // Re-enable pickup zone collider
    //    Transform pickupZone = transform.Find("PickupZone");
    //    if (pickupZone != null)
    //    {
    //        BoxCollider2D collider = pickupZone.GetComponent<BoxCollider2D>();
    //        if (collider != null)
    //        {
    //            collider.enabled = true;
    //        }
    //    }

    //    // Stop movement immediately before returning
    //    rb.linearVelocity = Vector2.zero;

    //    // Smoothly return to original pickup position
    //    StartCoroutine(SmoothReturnToPosition());
    //}

    public void DropMouse(Vector3 dropPosition)
    {
        isFollowingPlayer = false;
        isPickedUp = false;

        // Re-enable pickup zone collider
        Transform pickupZone = transform.Find("PickupZone");
        if (pickupZone != null)
        {
            BoxCollider2D collider = pickupZone.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }

        // Stop movement immediately
        rb.linearVelocity = Vector2.zero;

        // Drop the mouse at the new pickup location instead of the original spawn
        transform.position = dropPosition;

        // Reset mouse movement direction
        ResetMouseDirection();
    }

    private IEnumerator SmoothReturnToPosition()
    {
        float duration = 0.5f; // Time it takes to return
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, initialPickupPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPickupPosition; // Ensure exact positioning

        // Reset mouse movement direction
        ResetMouseDirection();
    }

    private void ResetMouseDirection()
    {
        // Check if there’s a wall on the right side
        bool isBlockedRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, LayerMask.GetMask("Ground"));

        // Check if there’s a wall on the left side
        bool isBlockedLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, LayerMask.GetMask("Ground"));

        if (isBlockedRight && !isBlockedLeft)
        {
            // If right is blocked, walk left
            walkDirectionVector = Vector2.left;
        }
        else if (isBlockedLeft && !isBlockedRight)
        {
            // If left is blocked, walk right
            walkDirectionVector = Vector2.right;
        }
        else
        {
            // Default to right if nothing is blocking either side
            walkDirectionVector = Vector2.right;
        }

        // Flip the sprite to match movement direction
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * Mathf.Sign(walkDirectionVector.x), transform.localScale.y);
    }


    public void FlipToMatchPlayer()
    {
        if (player != null)
        {
            // Flip mouse scale based on player's scale
            float playerScaleX = Mathf.Sign(player.localScale.x);
            transform.localScale = new Vector3(playerScaleX * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

}
