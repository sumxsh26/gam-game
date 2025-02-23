//using System;
//using UnityEngine;

//// ensures all enemy controllers require rigidbody and touching directions
//[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

//public class Knight : MonoBehaviour
//{
//    // walk speed of the enemy
//    public float walkSpeed = 3f;

//    public float walkStopRate = 0.05f;

//    public DetectionZone attackZone;
//    public DetectionZone cliffDetectionZone;

//    // adding rigidbody (unity component) to the script
//    Rigidbody2D rb;

//    // adding touching directions script to the script
//    TouchingDirections touchingDirections;

//    // adding animator to the script
//    Animator animator;

//    // adding damageable to the script
//    Damageable damageable;

//    // declaring enum representing the directions enemies can walk 
//    public enum WalkableDirection { Right, Left }

//    // stores the current walking direction
//    private WalkableDirection _walkDirection;

//    // enemies initialized to move the right
//    private Vector2 walkDirectionVector = Vector2.right;



//    // property for enemy walking directon
//    public WalkableDirection WalkDirection
//    {
//        // return the current direction enemy is walking towards
//        get { return _walkDirection; }

//        set {
//            // checks if the new direction is different from the current direction
//            if (_walkDirection != value)
//            {
//                // flip the enemy's sprite by inverting the x axis scale
//                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
//                    gameObject.transform.localScale.y);

//                // update which way the enemy moves based on the new walk direction
//                if (value == WalkableDirection.Right)
//                {
//                    // move right
//                    walkDirectionVector = Vector2.right;
//                }
//                else if (value == WalkableDirection.Left)
//                {
//                    // move left
//                    walkDirectionVector = Vector2.left;
//                }
//            }
//            // update the walk direction value
//            _walkDirection = value; }
//    }

//    public bool _hasTarget = false;

//    public bool HasTarget
//    {
//        get
//        {
//            return _hasTarget;
//        }
//        set
//        {
//            _hasTarget = value;
//            animator.SetBool(AnimationStrings.hasTarget, value);
//        }
//    }

//    public bool CanMove
//    {
//        get
//        {
//            return animator.GetBool(AnimationStrings.canMove);
//        }
//    }

//    public float AttackCooldown 
//    {
//        get 
//        {
//            return animator.GetFloat(AnimationStrings.attackCooldown);
//        }
//        private set
//        {
//            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
//        }
//    }

//    private void Awake()
//    {
//        // on awake, these components will be set (referenced from the components in Unity)
//        rb = GetComponent<Rigidbody2D>();
//        touchingDirections = GetComponent<TouchingDirections>();
//        animator = GetComponent<Animator>();
//        damageable = GetComponent<Damageable>();
//    }

//    private void FixedUpdate()
//    {

//        // if enemy is touching the wall and is on the ground
//        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall || cliffDetectionZone.detectedColliders.Count == 0) 
//        {
//            // flip the other way
//            FlipDirection();
//        }

//        if (!damageable.LockVelocity)
//        {
//            if (CanMove)
//            {
//                // set rigidbody velocity to apply movement
//                // moves enemy in the new direction on the same vertical velocity
//                rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.linearVelocity.y);
//            }
//            else
//            {
//                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
//            }
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        HasTarget = attackZone.detectedColliders.Count > 0;

//        if (AttackCooldown > 0)
//        {
//            AttackCooldown -= Time.deltaTime;
//        }
//    }

//    // flips the walking direction when hitting a wall
//    private void FlipDirection()
//    {


//        // check if enemy is facing right
//        if (_walkDirection == WalkableDirection.Right)
//        {
//            // change direction to left
//            WalkDirection = WalkableDirection.Left;
//        }

//        // check if enemy is facing left
//        else if (_walkDirection == WalkableDirection.Left)
//        {
//            // change direction to right
//            WalkDirection = WalkableDirection.Right;
//        }

//        // error handling
//        else
//        {
//            Debug.LogError("Current walkable direction is not set to legal values of right or left");
//        }
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    public void OnHit(int damage, Vector2 knockback)
//    {
//        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
//    }

//    //public void OnCliffDetected()
//    //{
//    //    if (touchingDirections.IsGrounded)
//    //    {
//    //        FlipDirection();
//    //    }
//    //}
//}



using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Damageable damageable;
    private BoxCollider2D platformCollider; // The collider for standing on the enemy

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    private float flipCooldown = 0.2f; // Small delay to prevent jittering
    private float lastFlipTime = 0f;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Flip sprite when changing direction
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.attackCooldown);
        private set => animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        // Get the platform collider from the child object (for standing on enemy)
        platformCollider = GetComponentInChildren<BoxCollider2D>();
    }


    private void FixedUpdate()
    {
        bool isBlockedByWall = touchingDirections.IsGrounded && touchingDirections.IsOnWall;
        bool isAtLedge = cliffDetectionZone.detectedColliders.Count == 0;

        // Only flip if truly needed, and ignore the player completely
        if (isBlockedByWall || isAtLedge)
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
            }
        }
    }




    private bool PlayerStandingOnTopSafely()
    {
        Vector2 checkPosition = (Vector2)transform.position + Vector2.up * 0.6f;

        // Use OverlapCircle to ensure the player is fully on top, not just touching the edge
        Collider2D hit = Physics2D.OverlapCircle(checkPosition, 0.3f, LayerMask.GetMask("Player"));

        if (hit != null)
        {
            Rigidbody2D playerRb = hit.GetComponent<Rigidbody2D>();

            // Ensure the player is actually resting on the enemy (not moving upwards)
            if (playerRb != null && playerRb.linearVelocity.y <= 0)
            {
                return true;
            }
        }

        return false;
    }

    private void FlipDirection()
    {
        // Ensure a minimum delay before flipping again
        if (Time.time - lastFlipTime < flipCooldown) return;

        WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
        lastFlipTime = Time.time; // Reset flip cooldown
    }


    // Ensure the platform collider remains active so the player can stand on the enemy
    private void LateUpdate()
    {
        if (platformCollider != null)
        {
            platformCollider.enabled = true;
        }
    }
}
