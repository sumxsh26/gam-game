using System;
using UnityEngine;

// ensures all enemy controllers require rigidbody and touching directions
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class Knight : MonoBehaviour
{
    // walk speed of the enemy
    public float walkSpeed = 3f;

    public float walkStopRate = 0.05f;

    public DetectionZone attackZone;

    // adding rigidbody (unity component) to the script
    Rigidbody2D rb;

    // adding touching directions script to the script
    TouchingDirections touchingDirections;

    // adding animator to the script
    Animator animator;

    // declaring enum representing the directions enemies can walk 
    public enum WalkableDirection { Right, Left }

    // stores the current walking direction
    private WalkableDirection _walkDirection;

    // enemies initialized to move the right
    private Vector2 walkDirectionVector = Vector2.right;

    Damageable damageable;


    // property for enemy walking directon
    public WalkableDirection WalkDirection
    {
        // return the current direction enemy is walking towards
        get { return _walkDirection; }

        set {
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
            _walkDirection = value; }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        // on awake, these components will be set (referenced from the components in Unity)
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
 
        // if enemy is touching the wall and is on the ground
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded) 
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

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
    }


}
