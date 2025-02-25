//using System;
//using System.Collections;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//// everytime a player controller is added, this ensures a rigidbody exists
//// you will not be able to add a player controller unless a rigidbody exists
//// you cannot remove rigidbody from playercontroller
//[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

//public class PlayerController : MonoBehaviour
//{
//    // adding rigidbody (unity component) to the script
//    public Rigidbody2D rb;

//    // adding animator (unity component) to the script
//    Animator animator;

//    // adding touching directions (script) to this script
//    public TouchingDirections touchingDirections;

//    // adding trail renederer (unity component) to the script
//    TrailRenderer trailRenderer;

//    CameraController cameraController;

//    Damageable damageable;

//    // adding Walking header in inspector
//    [Header("Walking")]

//    // how fast the player will walk left or right
//    public float walkSpeed = 10f;

//    // adding Running header in inspector
//    [Header("Running")]

//    // how fast the player will run left or right
//    public float runSpeed = 15f;

//    // adding Air Speed header in inspector
//    [Header("Air Speed")]

//    // how fast the player will move in the air
//    public float airWalkSpeed = 0.5f;

//    // adding Jumping header in inspector
//    [Header("Jumping")]
//    public float jumpImpulse = 8f;  // Lowered from 8f

//    [Header("Fall Speed Settings")]
//    public float fallMultiplier = 20f;      // Multiplies gravity when falling
//    public float lowJumpMultiplier = 12f;     // For shorter jumps when the jump button is released early

//    [Header("Max Fall Speed")]
//    public float maxFallSpeed = -100f;  // Was -45f, now -60f for **faster** long drops

//    //// adding Dashing header in inspector
//    //[Header("Dashing")]

//    //// the speed of the dash 
//    //// serializefield keeps the variable private but can be edited in inspector
//    //[SerializeField] private float dashingVelocity = 14f;

//    //// how long the dash will last
//    //[SerializeField] private float dashingTime = 0.5f;

//    //// store the direction of the dash
//    //private Vector2 dashingDir;

//    // store the movement input
//    Vector2 moveInput;

//    // for the fixed camera
//    private Vector3 initialPosition; // Stores the original spawn position
//    private bool positionCorrected = false; // Ensures we only correct position once

//    // for toggling : teleport mechanic - stores the last known safe position
//    private Vector2 lastSafePosition;

//    public KeyManager cm;
//    public event Action PlayerDied;


//    // happens when component exists inside of the scene (when you something to be found the moment the scene starts)
//    private void Awake()
//    {
//        // on awake, these components will be set (referenced from the components in Unity)
//        rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        trailRenderer = GetComponent<TrailRenderer>();
//        damageable = GetComponent<Damageable>();

//        // on awake, touching directions will be set
//        touchingDirections = GetComponent<TouchingDirections>();
//        //cameraController = GetComponent<CameraController>();

//        // prevent movement at scene start - for fixed camera
//        rb.bodyType = RigidbodyType2D.Kinematic;

//        // for toggling : teleport mechanic - start with the initial position
//        lastSafePosition = transform.position;

//    }


//    // Start is called once before the first execution of Update after the MonoBehaviour is created (comes after Awake)
//    void Start()
//    {
//        // for the fixed camera
//        // Store and log the initial position
//        initialPosition = transform.position;

//        // Start coroutine to check for unwanted position changes
//        StartCoroutine(CheckForPositionChange());
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // Track safe position whenever the player is on the ground
//        if (touchingDirections.IsGrounded) // Ensure ground detection works
//        {
//            lastSafePosition = transform.position;
//            Debug.Log("[SAFE POSITION] Updated to: " + lastSafePosition);
//        }
//    }


//    private void FixedUpdate()
//    {
//        if (!damageable.LockVelocity)
//        {
//            float targetSpeed;

//            if (touchingDirections.IsGrounded)
//            {
//                // Full movement on the ground
//                targetSpeed = moveInput.x * CurrentMoveSpeed;
//                rb.linearDamping = 0f;  // Reset drag when grounded
//            }
//            else
//            {
//                // Allow some air movement (but still limited)
//                targetSpeed = moveInput.x * airWalkSpeed;
//                rb.linearDamping = 3f;  // Slightly lower drag for faster movement
//            }

//            // Air movement restriction
//            float lerpFactor = touchingDirections.IsGrounded ? 1f : 0.1f;
//            float adjustedSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerpFactor);

//            // Limit max air movement
//            if (!touchingDirections.IsGrounded)
//            {
//                adjustedSpeed = Mathf.Clamp(adjustedSpeed, -3f, 3f); // Allow slight mid-air control
//            }

//            rb.linearVelocity = new Vector2(adjustedSpeed, rb.linearVelocity.y);
//        }

//        // **HYPER FAST FALLING**
//        if (rb.linearVelocity.y < 0) // Falling
//        {
//            // Exponentially increase fall speed over time
//            float boostedFallMultiplier = fallMultiplier + Mathf.Abs(rb.linearVelocity.y) * 0.1f;
//            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (boostedFallMultiplier - 1) * Time.fixedDeltaTime;

//            // **Set ultra-fast terminal velocity**
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -80f));
//        }
//        else if (rb.linearVelocity.y > 0 && !IsJumping()) // Released jump early
//        {
//            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -50f)); // Snappy drop
//        }

//        // ensure player lands smoothly on enemies
//        if (touchingDirections.IsGrounded && rb.linearVelocity.y == 0)
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
//        }

//        //if (IsDashing)
//        //{
//        //    rb.linearVelocity = new Vector2(dashingDir.x * dashingVelocity, rb.linearVelocity.y);
//        //    return;
//        //}

//        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
//    }

//    // to teleport the player to the last safe position
//    public void TeleportToSafePosition()
//    {
//        Debug.Log("[TELEPORT] Moving player to: " + lastSafePosition);

//        // Disable Collider to Prevent Sticking
//        Collider2D playerCollider = GetComponent<Collider2D>();
//        if (playerCollider != null)
//        {
//            playerCollider.enabled = false;
//        }

//        // Switch to Kinematic Mode to Prevent Physics Interference
//        rb.bodyType = RigidbodyType2D.Kinematic;

//        // Force Position Change
//        rb.position = lastSafePosition; // Direct teleport
//        rb.linearVelocity = Vector2.zero; // Reset movement to prevent unintended drift
//        rb.MovePosition(lastSafePosition); // Forces immediate physics update

//        // Restore Normal Physics After Teleporting
//        StartCoroutine(RestorePlayerPhysics());
//    }

//    private IEnumerator RestorePlayerPhysics()
//    {
//        yield return new WaitForSeconds(0.1f);

//        rb.bodyType = RigidbodyType2D.Dynamic; // Restore normal physics

//        Collider2D playerCollider = GetComponent<Collider2D>();
//        if (playerCollider != null)
//        {
//            playerCollider.enabled = true;
//        }

//        Debug.Log("[TELEPORT] Rigidbody and Collider restored.");
//    }


//    // checking if the position of the player changes (fixed camera)
//    private IEnumerator CheckForPositionChange()
//    {
//        yield return null; // Wait 1 frame for Unity to fully initialize everything
//        yield return new WaitForEndOfFrame(); // Extra wait for physics updates

//        Vector3 newPosition = transform.position;

//        // If the player's position has changed, log and correct it
//        if (newPosition != initialPosition && !positionCorrected)
//        {
//            transform.position = initialPosition; // Lock position back to original
//            positionCorrected = true;
//        }

//        rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
//    }

//    // this function triggers the correct move speed of the player based on certain conditions
//    public float CurrentMoveSpeed
//    {
//        get
//        {
//            // check if the player can move
//            if (CanMove)
//            {
//                // if player is moving and not touching a wall
//                if (IsMoving && !touchingDirections.IsOnWall)
//                {
//                    // if player is on the ground
//                    if (touchingDirections.IsGrounded)
//                    {
//                        // if run button (left shift) is held down
//                        if (IsRunning)
//                        {
//                            // trigger run speed
//                            return runSpeed;
//                        }
//                        else
//                        {
//                            // if not running, trigger walk speed
//                            return walkSpeed;
//                        }
//                    }
//                    // if not running or walking
//                    else
//                    {
//                        // trigger the air move speed
//                        return airWalkSpeed;
//                    }
//                }

//                // if not moving and touching a wall ( to ensure the player does not get stuck to the wall)
//                else
//                {
//                    // idle speed is 0
//                    return 0;
//                }
//            }

//            // if cannot move
//            else
//            {
//                // movement locked
//                return 0;
//            }
//        }
//    }

//    // set player movement to false (player does not move without input)
//    [SerializeField] private bool _isMoving = false;

//    // property for IsMoving in Unity (can use for idle or moving animation)
//    public bool IsMoving
//    {
//        // gets the value
//        get
//        {
//            // returns the current movement state
//            return _isMoving;
//        }

//        // updates and sets the value
//        private set
//        {
//            // updates the movement state
//            _isMoving = value;

//            // triggers the walk animation
//            animator.SetBool(AnimationStrings.isMoving, value);
//        }
//    }

//    // set player running to false (player does not run without input)
//    [SerializeField] private bool _isRunning = false;

//    // property for IsRunning in Unity
//    public bool IsRunning
//    {
//        // gets the value
//        get
//        {
//            // returns the current running state
//            return _isRunning;
//        }

//        // updates and sets the value
//        set
//        {
//            // updates the running state
//            _isRunning = value;

//            // triggers the run animation
//            animator.SetBool(AnimationStrings.isRunning, value);
//        }
//    }

//    //[SerializeField] private bool _isDashing = false;
//    //public bool IsDashing
//    //{
//    //    // gets the value
//    //    get
//    //    {
//    //        // returns the current dashing state
//    //        return _isDashing;
//    //    }

//    //    // updates and sets the value
//    //    set
//    //    {
//    //        // updates the dashing state
//    //        _isDashing = value;

//    //        // triggers the dash animation
//    //        animator.SetBool(AnimationStrings.isDashing, value);
//    //    }
//    //}

//    // set player to always face right when starting the game
//    public bool _isFacingRight = true;

//    // property for IsFacingRight in Unity
//    public bool IsFacingRight
//    {
//        // gets the value
//        get
//        {
//            // returns whether the player is facing right
//            return _isFacingRight;
//        }

//        // updates and flips the player if needed
//        private set
//        {
//            // only flip if new value is different from current direction
//            if (_isFacingRight != value)
//            {
//                // flip the local scale to make the player face the opposite direction
//                transform.localScale *= new Vector2(-1, 1);
//            }

//            // store teh new facing direction
//            _isFacingRight = value;
//        }
//    }

//    // property to check if player can move
//    public bool CanMove
//    {
//        get
//        {
//            // retrieves canMove boolean parameter from Animator in Unity
//            // returns true if movement is allowed, false if not
//            return animator.GetBool(AnimationStrings.canMove);
//        }
//    }

//    public bool IsAlive
//    {
//        get
//        {
//            return animator.GetBool(AnimationStrings.isAlive);
//        }
//    }



//    //getting move input
//    public void OnMove(InputAction.CallbackContext context)
//    {
//        // takes in the move input (x and y)
//        moveInput = context.ReadValue<Vector2>();

//        if (IsAlive)
//        {
//            // checks if the value is does not equal to zero to ensure player is moving
//            // if the value == zero, IsMoving is set to false
//            IsMoving = moveInput != Vector2.zero;

//            // sets the direction the player will face
//            SetFacingDirection(moveInput);
//        }
//        else
//        {
//            IsMoving = false;
//        }


//    }

//    // to ensure the player is facing the correct direction based on input
//    private void SetFacingDirection(Vector2 moveInput)
//    {
//        // if player is moving right and is not facing right
//        if (moveInput.x > 0 && !IsFacingRight)
//        {
//            // face the right
//            IsFacingRight = true;

//        }
//        // if player is moving left and is facing right
//        else if (moveInput.x < 0 && IsFacingRight)
//        {
//            // face the left
//            IsFacingRight = false;
//        }
//    }

//    // checks if the player is running based on input
//    public void OnRun(InputAction.CallbackContext context)
//    {
//        // if run button is pressed
//        if (context.started)
//        {
//            // player runs
//            IsRunning = true;
//        }
//        // if run button is not pressed
//        else if (context.canceled)
//        {
//            // player stops running
//            IsRunning = false;
//        }

//    }
//    //public void OnDash(InputAction.CallbackContext context)
//    //{
//    //    // Start dash only if not already dashing
//    //    if (context.started && !IsDashing)
//    //    {
//    //        StartCoroutine(PerformDash());
//    //        animator.SetTrigger(AnimationStrings.isDashing);
//    //    }
//    //}

//    // checks if player is jumping based on input
//    public void OnJump(InputAction.CallbackContext context)
//    {
//        // if jump button is pressed and player is on the ground and can move
//        if (context.started && touchingDirections.IsGrounded && CanMove)
//        {
//            // trigger the jump animation
//            animator.SetTrigger(AnimationStrings.jumpTrigger);

//            // apply upward force to the rigidbody
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
//        }
//    }

//    private bool IsJumping()
//    {
//        return Input.GetButton("Jump"); // Works with default Unity input settings
//    }

//    // checks if player is attacking based on input
//    public void OnAttack(InputAction.CallbackContext context)
//    {
//        // if attack button is pressed
//        if (context.started)
//        {
//            // trigger attack animation
//            animator.SetTrigger(AnimationStrings.attackTrigger);
//        }
//    }

//    public void OnHit(int damage, Vector2 knockback)
//    {
//        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
//    }

//    //[SerializeField] private bool canDash = true;

//    //private IEnumerator PerformDash()
//    //{
//    //    if (!canDash) yield break; // Prevent dashing if cooldown is active

//    //    // disable further dashing
//    //    canDash = false;

//    //    // mark player as dashing
//    //    IsDashing = true;

//    //    // enable trail effect
//    //    trailRenderer.emitting = true;

//    //    // determine dash direction based on player's facing direction
//    //    dashingDir = IsFacingRight ? Vector2.right : Vector2.left;

//    //    // apply dash velocity
//    //    rb.linearVelocity = new Vector2(dashingDir.x * dashingVelocity, 0); // zero vertical velocity

//    //    yield return new WaitForSeconds(dashingTime); // dash duration

//    //    // reset velocity after dash ends
//    //    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

//    //    // disable trail effect
//    //    trailRenderer.emitting = false; // Disable trail effect
//    //    IsDashing = false; // Dash state ends

//    //    // Add cooldown to prevent spamming
//    //    yield return new WaitForSeconds(1f);
//    //    canDash = true; // Enable dash again
//    //}

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // count coin, destroy door
//        if (other.gameObject.CompareTag("Key"))
//        {
//            Destroy(other.gameObject);
//            cm.keyCount++;
//        }

//        // stationary spike damage, use OnTrigger so that the player does not bounce off the spike
//        else if (other.gameObject.CompareTag("Spike"))
//        {
//            // deal 1 heart when player hits a spike
//            damageable.Hit(1, Vector2.zero);
//        }

//        // water hazard - player drowns
//        else if (other.gameObject.CompareTag("Water"))
//        {
//            // deal fatal damage to drown the player
//            damageable.Hit(damageable.Health, Vector2.zero);
//        }
//    }

//    // falling spike damage
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Spike"))
//        {
//            // deal 1 heart per spike hit
//            damageable.Hit(1, Vector2.zero);

//            //destroy player once hits spikes
//            //SceneManager.LoadScene("GameOver");

//            //PlayerDied.Invoke(); // triggers GameControllerScript!
//        }
//    }

//    public void TriggerPlayerDeath()
//    {
//        // Safely trigger the PlayerDied event
//        // other scripts can just call it like this: gameController.PlayerController.TriggerPlayerDeath();
//        // refer to TimeManagerScript
//        PlayerDied?.Invoke();
//    }


//}




// updated script
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// everytime a player controller is added, this ensures a rigidbody exists
// you will not be able to add a player controller unless a rigidbody exists
// you cannot remove rigidbody from playercontroller
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    // adding rigidbody (unity component) to the script
    public Rigidbody2D rb;

    // adding animator (unity component) to the script
    Animator animator;

    // adding touching directions (script) to this script
    public TouchingDirections touchingDirections;

    Damageable damageable;

    // adding Walking header in inspector
    [Header("Walking")]

    // how fast the player will walk left or right
    public float walkSpeed = 10f;

    // adding Running header in inspector
    [Header("Running")]

    // how fast the player will run left or right
    public float runSpeed = 15f;

    // adding Air Speed header in inspector
    [Header("Air Speed")]

    // how fast the player will move in the air
    public float airWalkSpeed = 0.5f;

    // adding Jumping header in inspector
    [Header("Jumping")]
    public float jumpImpulse = 8f;  // Lowered from 8f

    [Header("Fall Speed Settings")]
    public float fallMultiplier = 20f;      // Multiplies gravity when falling
    public float lowJumpMultiplier = 12f;     // For shorter jumps when the jump button is released early

    [Header("Max Fall Speed")]
    public float maxFallSpeed = -100f;  // Was -45f, now -60f for **faster** long drops

    // store the movement input
    Vector2 moveInput;

    // for the fixed camera
    private Vector3 initialPosition; // Stores the original spawn position
    private bool positionCorrected = false; // Ensures we only correct position once

    // for toggling : teleport mechanic - stores the last known safe position
    private Vector2 lastSafePosition;

    // for mice collection
    public List<Mice> collectedMice = new List<Mice>();


    public Key cm;
    public event Action PlayerDied;


    // happens when component exists inside of the scene (when you something to be found the moment the scene starts)
    private void Awake()
    {
        // on awake, these components will be set (referenced from the components in Unity)
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        // on awake, touching directions will be set
        touchingDirections = GetComponent<TouchingDirections>();

        // prevent movement at scene start - for fixed camera
        rb.bodyType = RigidbodyType2D.Kinematic;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created (comes after Awake)
    void Start()
    {
        // for the fixed camera
        // Store and log the initial position
        initialPosition = transform.position;

        // Start coroutine to check for unwanted position changes
        StartCoroutine(CheckForPositionChange());
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            float targetSpeed;

            if (touchingDirections.IsGrounded)
            {
                // Full movement on the ground
                targetSpeed = moveInput.x * CurrentMoveSpeed;
                rb.linearDamping = 0f;  // Reset drag when grounded
            }
            else
            {
                // Allow some air movement (but still limited)
                targetSpeed = moveInput.x * airWalkSpeed;
                rb.linearDamping = 3f;  // Slightly lower drag for faster movement
            }

            // Air movement restriction
            float lerpFactor = touchingDirections.IsGrounded ? 1f : 0.1f;
            float adjustedSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerpFactor);

            // Limit max air movement
            if (!touchingDirections.IsGrounded)
            {
                adjustedSpeed = Mathf.Clamp(adjustedSpeed, -3f, 3f); // Allow slight mid-air control
            }

            rb.linearVelocity = new Vector2(adjustedSpeed, rb.linearVelocity.y);
        }

        // **HYPER FAST FALLING**
        if (rb.linearVelocity.y < 0) // Falling
        {
            // Exponentially increase fall speed over time
            float boostedFallMultiplier = fallMultiplier + Mathf.Abs(rb.linearVelocity.y) * 0.1f;
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (boostedFallMultiplier - 1) * Time.fixedDeltaTime;

            // **Set ultra-fast terminal velocity**
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -80f));
        }

        // Ensure player lands smoothly on enemies
        if (touchingDirections.IsGrounded && rb.linearVelocity.y == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }


    // checking if the position of the player changes (fixed camera)
    private IEnumerator CheckForPositionChange()
    {
        yield return null; // Wait 1 frame for Unity to fully initialize everything
        yield return new WaitForEndOfFrame(); // Extra wait for physics updates

        Vector3 newPosition = transform.position;

        // If the player's position has changed, log and correct it
        if (newPosition != initialPosition && !positionCorrected)
        {
            transform.position = initialPosition; // Lock position back to original
            positionCorrected = true;
        }

        rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
    }

    // this function triggers the correct move speed of the player based on certain conditions
    public float CurrentMoveSpeed
    {
        get
        {
            // check if the player can move
            if (CanMove)
            {
                // if player is moving and not touching a wall
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    // if player is on the ground
                    if (touchingDirections.IsGrounded)
                    {
                        // if run button (left shift) is held down
                        if (IsRunning)
                        {
                            // trigger run speed
                            return runSpeed;
                        }
                        else
                        {
                            // if not running, trigger walk speed
                            return walkSpeed;
                        }
                    }
                    // if not running or walking
                    else
                    {
                        // trigger the air move speed
                        return airWalkSpeed;
                    }
                }

                // if not moving and touching a wall ( to ensure the player does not get stuck to the wall)
                else
                {
                    // idle speed is 0
                    return 0;
                }
            }

            // if cannot move
            else
            {
                // movement locked
                return 0;
            }
        }
    }

    // set player movement to false (player does not move without input)
    [SerializeField] private bool _isMoving = false;

    // property for IsMoving in Unity (can use for idle or moving animation)
    public bool IsMoving
    {
        // gets the value
        get
        {
            // returns the current movement state
            return _isMoving;
        }

        // updates and sets the value
        private set
        {
            // updates the movement state
            _isMoving = value;

            // triggers the walk animation
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    // set player running to false (player does not run without input)
    [SerializeField] private bool _isRunning = false;

    // property for IsRunning in Unity
    public bool IsRunning
    {
        // gets the value
        get
        {
            // returns the current running state
            return _isRunning;
        }

        // updates and sets the value
        set
        {
            // updates the running state
            _isRunning = value;

            // triggers the run animation
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    // set player to always face right when starting the game
    public bool _isFacingRight = true;

    // property for IsFacingRight in Unity
    public bool IsFacingRight
    {
        // gets the value
        get
        {
            // returns whether the player is facing right
            return _isFacingRight;
        }

        // updates and flips the player if needed
        private set
        {
            // only flip if new value is different from current direction
            if (_isFacingRight != value)
            {
                // flip the local scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            // store teh new facing direction
            _isFacingRight = value;
        }
    }

    // property to check if player can move
    public bool CanMove
    {
        get
        {
            // retrieves canMove boolean parameter from Animator in Unity
            // returns true if movement is allowed, false if not
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }



    //getting move input
    public void OnMove(InputAction.CallbackContext context)
    {
        // takes in the move input (x and y)
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            // checks if the value is does not equal to zero to ensure player is moving
            // if the value == zero, IsMoving is set to false
            IsMoving = moveInput != Vector2.zero;

            // sets the direction the player will face
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }


    }

    // to ensure the player is facing the correct direction based on input
    private void SetFacingDirection(Vector2 moveInput)
    {
        // if player is moving right and is not facing right
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // face the right
            IsFacingRight = true;

        }
        // if player is moving left and is facing right
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // face the left
            IsFacingRight = false;
        }
    }

    // checks if the player is running based on input
    public void OnRun(InputAction.CallbackContext context)
    {
        // if run button is pressed
        if (context.started)
        {
            // player runs
            IsRunning = true;
        }
        // if run button is not pressed
        else if (context.canceled)
        {
            // player stops running
            IsRunning = false;
        }

    }


    // checks if player is jumping based on input
    public void OnJump(InputAction.CallbackContext context)
    {
        // if jump button is pressed and player is on the ground and can move
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            // trigger the jump animation
            animator.SetTrigger(AnimationStrings.jumpTrigger);

            // apply upward force to the rigidbody
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }


    // checks if player is attacking based on input
    public void OnAttack(InputAction.CallbackContext context)
    {
        // if attack button is pressed
        if (context.started)
        {
            // trigger attack animation
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    // for enemy attack 
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
    }

    // for mice collection
    [SerializeField] private bool _isCatchingMice = false;

    public bool IsCatchingMice
    {
        get { return _isCatchingMice; }
    }

    // Checks if the player is holding Shift to catch mice
    public void OnCatchMice(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isCatchingMice = true;
        }
        else if (context.canceled)
        {
            _isCatchingMice = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // count coin, destroy door
        if (other.gameObject.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            cm.keyCount++;
        }

        // stationary spike damage, use OnTrigger so that the player does not bounce off the spike
        else if (other.gameObject.CompareTag("Spike"))
        {
            // deal 1 heart when player hits a spike
            damageable.Hit(1, Vector2.zero);
        }

        // water hazard - player drowns
        else if (other.gameObject.CompareTag("Water"))
        {
            // deal fatal damage to drown the player
            damageable.Hit(damageable.Health, Vector2.zero);
        }

        // Picking up a mouse
        else if (other.gameObject.CompareTag("Mice"))
        {
            Mice mouse = other.GetComponent<Mice>();

            if (mouse != null && !collectedMice.Contains(mouse))
            {
                // Ensure the follow target exists
                Transform miceFollowPoint = transform.Find("MiceFollowPoint");
                if (miceFollowPoint != null)
                {
                    collectedMice.Add(mouse);
                    mouse.followTarget = miceFollowPoint; // Set follow target
                    mouse.isFollowingPlayer = true; // Ensure the mouse starts following

                    Debug.Log("Mouse picked up and is now following the player.");
                }
                else
                {
                    Debug.LogError("MiceFollowPoint not found! Ensure it exists in the player hierarchy.");
                }
            }
        }

        // Depositing mice into a cage
        else if (other.gameObject.CompareTag("Cage"))
        {
            Cage cage = other.GetComponent<Cage>();
            if (cage != null && collectedMice.Count > 0)
            {
                Mice depositedMouse = collectedMice[0]; // Take the first mouse
                collectedMice.RemoveAt(0); // Remove from list

                // Call the cage's method to activate ALL TilemapToggles
                cage.ActivateAllTilemapToggles();

                Debug.Log("Mouse deposited into cage.");

                Destroy(depositedMouse.gameObject); // Remove mouse from scene
            }
        }
    }


    // falling spike damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            // deal 1 heart per spike hit
            damageable.Hit(1, Vector2.zero);

            //destroy player once hits spikes
            //SceneManager.LoadScene("GameOver");

            //PlayerDied.Invoke(); // triggers GameControllerScript!
        }
    }

    public void TriggerPlayerDeath()
    {
        // Safely trigger the PlayerDied event
        PlayerDied?.Invoke();
    }

}