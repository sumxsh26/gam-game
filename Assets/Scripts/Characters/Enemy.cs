using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Enemy : MonoBehaviour
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
