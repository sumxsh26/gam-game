using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector2 Move;
}

public interface IPlayerController
{
    public event Action<bool, float> GroundedChanged;

    public event Action Jumped;
    public Vector2 FrameInput { get; }
}

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] private ScriptableStats _stats;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    private bool _cachedQueryStartInColliders;

    #region Interface

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    #endregion

    private float _time;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
    }

    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
            JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        };

        if (_stats.SnapInput)
        {
            _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
            _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        }

        if (_frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        HandleJump();
        HandleDirection();
        HandleGravity();

        ApplyMovement();
    }

    #region Collisions

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
        bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

        // Hit a Ceiling
        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    #endregion


    #region Jumping

    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote) ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = _stats.JumpPower;
        Jumped?.Invoke();
    }

    #endregion

    #region Horizontal

    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
    }

    #endregion

    #region Gravity

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _stats.GroundingForce;
        }
        else
        {
            var inAirGravity = _stats.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    #endregion

    private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif



    // for mice collection
    public List<Mice> collectedMice = new List<Mice>();

    // adding animator (unity component) to the script
    Animator animator;

    // adding touching directions (script) to this script
    public TouchingDirections touchingDirections;

    Damageable damageable;

    public Key cm;
    public event Action PlayerDied;

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
        _rb.linearVelocity = new Vector2(knockback.x, _rb.linearVelocity.y + knockback.y);
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
                UnityEngine.Transform miceFollowPoint = transform.Find("MiceFollowPoint");
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
        }
    }

    public void TriggerPlayerDeath()
    {
        // Safely trigger the PlayerDied event
        PlayerDied?.Invoke();
    }
}

