using System;
using UnityEngine;

// ensures all enemy controllers require rigidbody and touching directions
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class Enemy : MonoBehaviour
{
    // walk speed of the enemy
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;

    // awareness zone chase speed
    public float chaseSpeedMultiplier = 1.5f; // Speed multiplier when chasing

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    // awareness zone
    public DetectionZone awarenessZone;
    private float awarenessFlipCooldown = 1.5f; // Time in seconds between awareness flips
    private float nextAllowedAwarenessFlipTime = 0f;

    // adding rigidbody (unity component) to the script
    Rigidbody2D rb;

    // adding touching directions script to the script
    TouchingDirections touchingDirections;

    // adding animator to the script
    Animator animator;

    // adding damageable to the script
    Damageable damageable;

    // declaring enum representing the directions enemies can walk 
    public enum WalkableDirection { Right, Left }

    // stores the current walking direction
    private WalkableDirection _walkDirection;

    // enemies initialized to move the right
    private Vector2 walkDirectionVector = Vector2.right;



    // property for enemy walking directon
    public WalkableDirection WalkDirection
    {
        // return the current direction enemy is walking towards
        get { return _walkDirection; }

        set
        {
            // checks if the new direction is different from the current direction
            if (_walkDirection != value)
            {
                // flip the enemy's sprite by inverting the x axis scale
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);

                // update which way the enemy moves based on the new walk direction
                if (value == WalkableDirection.Right)
                {
                    // move right
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    // move left
                    walkDirectionVector = Vector2.left;
                }
            }
            // update the walk direction value
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get => _hasTarget;
        set
        {
            if (_hasTarget == value) return;

            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);

            // Play enemy attack sound only when entering "HasTarget = true"
            if (_hasTarget)
            {
                AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
                if (audioManager != null && audioManager.enemyHit != null)
                {
                    audioManager.PlaySFX(audioManager.enemyHit);
                }
            }
        }
    }


    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        // on awake, these components will be set (referenced from the components in Unity)
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {

        // if enemy is touching the wall and is on the ground
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall || cliffDetectionZone.detectedColliders.Count == 0)
        {
            // flip the other way
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                // set rigidbody velocity to apply movement
                // moves enemy in the new direction on the same vertical velocity
                rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
            }
        }
    }

    private void Update()
    {
        bool playerDetectedInAwareness = false;

        // Awareness zone logic
        foreach (Collider2D col in awarenessZone.detectedColliders)
        {
            PlayerMovement player = col.GetComponent<PlayerMovement>();
            if (player != null && player.IsAlive)
            {
                playerDetectedInAwareness = true;

                // Flip if player is behind AND cooldown has passed
                bool playerIsLeft = player.transform.position.x < transform.position.x;
                if ((playerIsLeft && WalkDirection == WalkableDirection.Right) ||
                    (!playerIsLeft && WalkDirection == WalkableDirection.Left))
                {
                    if (Time.time >= nextAllowedAwarenessFlipTime)
                    {
                        FlipDirection();
                        nextAllowedAwarenessFlipTime = Time.time + awarenessFlipCooldown;
                    }
                }

                break; // Stop after the first valid player found
            }
        }

        // Attack zone logic
        if (attackZone.detectedColliders.Count > 0)
        {
            PlayerMovement player = attackZone.detectedColliders[0].GetComponent<PlayerMovement>();

            if (player != null && player.IsAlive)
            {
                HasTarget = true;
            }
            else
            {
                HasTarget = false;
            }
        }
        else
        {
            HasTarget = false;
        }

        // Cooldown logic
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        // Adjust speed based on awareness
        walkSpeed = playerDetectedInAwareness ? 5f : 3f;
    }



    public void StopTargetingPlayer()
    {
        HasTarget = false;
        Debug.Log("[DEBUG] Enemy stopped attacking because player died.");
    }


    // flips the walking direction when hitting a wall
    private void FlipDirection()
    {


        // check if enemy is facing right
        if (_walkDirection == WalkableDirection.Right)
        {
            // change direction to left
            WalkDirection = WalkableDirection.Left;
        }

        // check if enemy is facing left
        else if (_walkDirection == WalkableDirection.Left)
        {
            // change direction to right
            WalkDirection = WalkableDirection.Right;
        }

        // error handling
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
    }

}