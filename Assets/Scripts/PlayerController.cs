using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

// anything i add a player controller, this makes sure a rigidbody exists
// makes sure that you cannot add a player controller unless a rigidbody exists 
// makes sure you cannot remove rigidbody from playercontroller
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    // how fast avatar will walk left or right
    public float walkSpeed = 10f;
    public float runSpeed = 15f;

    Vector2 moveInput;

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving)
            {
                if (IsRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }
            else
            {
                // idle speed is 0
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    // property for IsMoving (can use for idle or moving animation)
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                // flip the local scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    // adding rigidbody to the script
    Rigidbody2D rb;
    Animator animator;

    // happens when component exists inside of the scene (when you something to be found the moment the scene starts)
    private void Awake()
    {
        // on awake, the RigidBody is going to be set (referenced from the Rigidbody in Unity)
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created (comes after Awake)
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // for physics updates
    private void FixedUpdate()
    {
        // moveInput.x == the input from the player on the x axis
        // Time.fixedDeltaTime makes sure the movement is consistent (but is handled with velocity)
        // y velocity is controlled by gravity
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
    }

    //getting move input
    public void OnMove(InputAction.CallbackContext context)
    {
        // each time move input (x and y)
        moveInput = context.ReadValue<Vector2>();

        // check if the value is not == zero to make sure is moving, if not set IsMoving to false
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // face the right
            IsFacingRight = true;

        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }

    }
}
