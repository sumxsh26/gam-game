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
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Mice : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float escapeSpeed = 6f; // Speed when escaping from the player
    public float walkStopRate = 0.05f; // Smooth deceleration
    public DetectionZone playerDetectionZone; // Detects the player to run away
    public DetectionZone cliffDetectionZone; // Detects ledges

    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Vector2 walkDirectionVector = Vector2.right;
    private float flipCooldown = 0.2f; // Delay to prevent jittering
    private float lastFlipTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();

        if (touchingDirections == null)
            Debug.LogError("TouchingDirections component is missing on: " + gameObject.name);

        if (cliffDetectionZone == null)
            Debug.LogError("CliffDetectionZone is not assigned in: " + gameObject.name);

        if (playerDetectionZone == null)
            Debug.LogError("PlayerDetectionZone is not assigned in: " + gameObject.name);
    }

    private void FixedUpdate()
    {
        MoveMouse();
    }

    private void MoveMouse()
    {
        if (touchingDirections == null || cliffDetectionZone == null)
            return;

        bool isBlockedByWall = touchingDirections.IsGrounded && touchingDirections.IsOnWall;
        bool isAtLedge = cliffDetectionZone.detectedColliders.Count == 0;

        // Flip if the mouse hits a wall or reaches a ledge
        if ((isBlockedByWall || isAtLedge) && Time.time - lastFlipTime > flipCooldown)
        {
            FlipDirection();
        }

        float currentSpeed = walkSpeed;

        // If the player is nearby, make the mouse run away faster AND flip direction
        if (playerDetectionZone.detectedColliders.Count > 0)
        {
            currentSpeed = escapeSpeed;
            FlipDirection(); // Flip to run away from the player
        }

        rb.linearVelocity = new Vector2(currentSpeed * walkDirectionVector.x, rb.linearVelocity.y);
    }

    private void FlipDirection()
    {
        if (Time.time - lastFlipTime < flipCooldown) return;

        // Flip movement direction
        walkDirectionVector = -walkDirectionVector;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        lastFlipTime = Time.time; // Reset flip cooldown
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected! Picking up mouse.");

            // Toggle platforms upon pickup
            TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
            foreach (TilemapToggle toggle in toggles)
            {
                toggle.TogglePlatform();
            }

            Destroy(gameObject); // Destroy the mouse after pickup
        }
    }
}

