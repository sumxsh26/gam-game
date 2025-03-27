//using System;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{

//    [Header("References")]
//    public PlayerMovementStats MoveStats;
//    [SerializeField] private Collider2D _feetColl;
//    [SerializeField] private Collider2D _bodyColl;

//    private Rigidbody2D _rb;

//    // movement variables
//    private Vector2 _moveVelocity;
//    private bool _isFacingRight;

//    // collision check vars
//    private RaycastHit2D _groundHit;
//    private RaycastHit2D _headHit;
//    private bool _isGrounded;
//    private bool _bumpedHead;

//    // jump variables
//    public float VerticalVelocity { get; private set; }
//    private bool _isJumping;
//    private bool _isFastFalling;
//    private bool _isFalling;
//    private float _fastFallTime;
//    private float _fastFallReleaseSpeed;
//    private int _numberOfJumpsUsed;

//    // apex variables
//    private float _apexPoint;
//    private float _timePastApexThreshold;
//    private bool _isPastApexThreshold;

//    // jump buffer variables
//    private float _jumpBufferTimer;
//    private bool _jumpReleasedDuringBuffer;

//    // coyote time variables
//    private float _coyoteTimer;


//    private void Awake()
//    {
//        _isFacingRight = true;

//        _rb = GetComponent<Rigidbody2D>();
//    }

//    private void FixedUpdate()
//    {
//        CollisionChecks();
//        Jump();

//        if (_isGrounded)
//        {
//            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
//        }
//        else
//        {
//            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
//        }
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        CountTimers();
//        JumpChecks();
//    }
//    #region Movement

//    private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    {
//        if (moveInput != Vector2.zero)
//        {
//            TurnCheck(moveInput);

//            Vector2 targetVelocity = Vector2.zero;

//            if (InputManager.RunIsHeld)
//            {
//                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
//            }

//            else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

//            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//        else if (moveInput == Vector2.zero) 
//        {
//            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//    }

//    private void TurnCheck(Vector2 moveInput)
//    {
//        if (_isFacingRight && moveInput.x < 0)
//        {
//            Turn(false);
//        }

//        else if (!_isFacingRight && moveInput.x > 0) 
//        {
//            Turn(true);
//        }
//    }

//    private void Turn(bool turnRight)
//    {
//        if (turnRight)
//        {
//            _isFacingRight = true;
//            transform.Rotate(0f, 180f, 0f);
//        }
//        else
//        {
//            _isFacingRight = false;
//            transform.Rotate(0f, -180f, 0f);
//        }
//    }

//    #endregion

//    #region Jump
//    private void JumpChecks()
//    {
//        // when jump is pressed
//        if (InputManager.JumpWasPressed)
//        {
//            _jumpBufferTimer = MoveStats.JumpBufferTime;
//            _jumpReleasedDuringBuffer = false;
//        }

//        // when jump is released
//        if (InputManager.JumpWasReleased)
//        {
//            if(_jumpBufferTimer > 0f)
//            {
//                _jumpReleasedDuringBuffer = true;
//            }

//            if (_isJumping && VerticalVelocity > 0f)
//            {
//                if (_isPastApexThreshold)
//                {
//                    _isPastApexThreshold = false;
//                    _isFastFalling = true;
//                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
//                    VerticalVelocity = 0f;
//                }
//                else
//                {
//                    _isFastFalling = true;
//                    _fastFallReleaseSpeed = VerticalVelocity;
//                }
//            }
//        }

//        // initiate jump with jump buffering and coyote time
//        if(_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
//        {
//            InitiateJump(1);

//            if (_jumpReleasedDuringBuffer)
//            {
//                _isFastFalling = true;
//                _fastFallReleaseSpeed = VerticalVelocity;
//            }
//        }

//        // double jump
//        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
//        {
//            _isFastFalling = false;
//            InitiateJump(1);
//        }

//        // handle air jump after the coyote time has lapsed (take off an extra jump so the player does not get a bonus jump)
//        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
//        {
//            InitiateJump(2);
//            _isFastFalling = false;
//        }

//        // landing
//        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
//        {
//            _isJumping = false;
//            _isFalling = false;
//            _isFastFalling = false;
//            _fastFallTime = 0f;
//            _isPastApexThreshold = false;
//            _numberOfJumpsUsed = 0;

//            VerticalVelocity = Physics2D.gravity.y;
//        }
//    }

//    private void InitiateJump(int numberOfJumpsUsed)
//    {
//        if (!_isJumping)
//        {
//            _isJumping = true;
//        }

//        _jumpBufferTimer = 0f;
//        _numberOfJumpsUsed += numberOfJumpsUsed;
//        VerticalVelocity = MoveStats.InitialJumpVelocity;
//    }

//    private void Jump()
//    {
//        // apply gravity while jumping
//        if (_isJumping) 
//        {
//            // check for head bump
//            if (_bumpedHead)
//            {
//                _isFastFalling = true;
//            }

//            // gravity on ascending 
//            if (VerticalVelocity >= 0f)
//            {
//                // apex controls
//                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

//                if (_apexPoint > MoveStats.ApexThreshold)
//                {
//                    if (!_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = true;
//                        _timePastApexThreshold = 0f;
//                    }

//                    if (_isPastApexThreshold)
//                    {
//                        _timePastApexThreshold += Time.fixedDeltaTime;

//                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
//                        {
//                            VerticalVelocity = 0f;
//                        }
//                        else
//                        {
//                            VerticalVelocity = -0.01f;
//                        }
//                    }
//                }
//                // gravity on descending but not past apex threshold
//                else
//                {
//                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//                    if (_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = false;
//                    }
//                }
//            }

//            // gravity on descending
//            else if (!_isFastFalling)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }

//            else if (VerticalVelocity < 0f)
//            {
//                if (!_isFalling) 
//                {
//                    _isFalling = true;
//                }
//            }
//        }

//        // jump cut
//        if(_isFastFalling)
//        {
//            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }
//            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
//            }

//            _fastFallTime += Time.fixedDeltaTime;
//        }

//        // normal gravity while falling
//        if(!_isGrounded && !_isJumping)
//        {
//            if (!_isFalling)
//            {
//                _isFalling = true;
//            }

//            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//        }

//        // clamp fall speed
//        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 80f);

//        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
//    }

//    #endregion

//    #region Collision Checks

//    private void IsGrounded()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

//        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

//        if (_groundHit.collider != null)
//        {
//            _isGrounded = true;
//        }
//        else { _isGrounded = false; }

//        #region Debug Visualizaton
//        if (MoveStats.DebugShowIsGroundedBox)
//        {
//            Color rayColor;
//            if (_isGrounded)
//            {
//                rayColor = Color.green;
//            }
//            else { rayColor = Color.red; }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
//        }
//        #endregion
//    }

//    private void BumpedHead()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

//        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

//        if (_headHit.collider != null)
//        {
//            _bumpedHead = true;
//        }
//        else { _bumpedHead = false; }

//        #region Debug Visualizaton 
//        if (MoveStats.DebugShowHeadBumpBox)
//        {
//            float headWidth = MoveStats.HeadWidth;

//            Color rayColor;
//            if (_bumpedHead)
//            {
//                rayColor = Color.green;
//            }
//            else
//            {
//                rayColor= Color.red;
//            }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
//        }

//        #endregion 
//    }

//    private void CollisionChecks()
//    {
//        IsGrounded();
//        BumpedHead();
//    }
//    #endregion

//    #region Timers

//    private void CountTimers()
//    {
//        _jumpBufferTimer -= Time.deltaTime;

//        if (!_isGrounded)
//        {
//            _coyoteTimer -= Time.deltaTime;
//        }
//        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
//    }

//    #endregion
//}


//using System;
//using System.Collections;
//using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

//public class PlayerMovement : MonoBehaviour
//{

//    [Header("References")]
//    public PlayerMovementStats MoveStats;
//    [SerializeField] private Collider2D _feetColl;
//    [SerializeField] private Collider2D _bodyColl;

//    private Rigidbody2D _rb;
//    private Animator animator;
//    private Damageable damageable;

//    // movement variables
//    private Vector2 _moveVelocity;
//    public bool _isFacingRight;

//    // collision check vars
//    private RaycastHit2D _groundHit;
//    private RaycastHit2D _headHit;
//    private bool _isGrounded;
//    private bool _bumpedHead;

//    // jump variables
//    public float VerticalVelocity { get; private set; }
//    private bool _isJumping;
//    private bool _isFastFalling;
//    private bool _isFalling;
//    private float _fastFallTime;
//    private float _fastFallReleaseSpeed;
//    private int _numberOfJumpsUsed;

//    // apex variables
//    private float _apexPoint;
//    private float _timePastApexThreshold;
//    private bool _isPastApexThreshold;

//    // jump buffer variables
//    private float _jumpBufferTimer;
//    private bool _jumpReleasedDuringBuffer;

//    // coyote time variables
//    private float _coyoteTimer;

//    // for the fixed camera
//    private Vector3 initialPosition; // Stores the original spawn position
//    private bool positionCorrected = false; // Ensures we only correct position once

//    // key collection
//    public Key cm;
//    public event Action PlayerDied;


//    private void Awake()
//    {
//        _isFacingRight = true;

//        _rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        damageable = GetComponent<Damageable>();

//        // prevent movement at scene start - for fixed camera
//        _rb.bodyType = RigidbodyType2D.Kinematic;
//    }

//    private void FixedUpdate()
//    {

//            CollisionChecks();
//            Jump();

//            if (_isGrounded)
//            {
//                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
//            }
//            else
//            {
//                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
//            }
//        // animator
//        animator.SetBool(AnimationStrings.isMoving, InputManager.Movement.x != 0);
//        animator.SetBool(AnimationStrings.isGrounded, _isGrounded);
//        animator.SetFloat(AnimationStrings.yVelocity, _rb.linearVelocity.y);

//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
//        CountTimers();
//        JumpChecks();
//    }

//    #region Camera
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

//        _rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
//    }

//    #endregion

//    #region Movement

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

//    private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    {
//        // Prevent movement if the player cannot move or is dead
//        if (!CanMove || !IsAlive)
//        {
//            _moveVelocity = Vector2.zero;
//            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//            return;
//        }

//        if (moveInput != Vector2.zero)
//        {
//            TurnCheck(moveInput);

//            Vector2 targetVelocity = Vector2.zero;

//            if (InputManager.RunIsHeld)
//            {
//                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
//            }

//            else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

//            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//        else if (moveInput == Vector2.zero)
//        {
//            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//    }

//    private void TurnCheck(Vector2 moveInput)
//    {
//        bool shouldFaceRight = moveInput.x > 0;
//        bool shouldFaceLeft = moveInput.x < 0;

//        if (shouldFaceRight && !_isFacingRight)
//        {
//            Flip();
//        }
//        else if (shouldFaceLeft && _isFacingRight)
//        {
//            Flip();
//        }
//    }

//    // animator
//    private void Flip()
//    {
//        _isFacingRight = !_isFacingRight;
//        transform.localScale *= new Vector2(-1, 1);
//    }

//    private void Turn(bool turnRight)
//    {
//        if (turnRight)
//        {
//            _isFacingRight = true;
//            transform.Rotate(0f, 180f, 0f);
//        }
//        else
//        {
//            _isFacingRight = false;
//            transform.Rotate(0f, -180f, 0f);
//        }
//    }

//    #endregion

//    #region Jump
//    private void JumpChecks()
//    {
//        // when jump is pressed
//        if (InputManager.JumpWasPressed)
//        {
//            _jumpBufferTimer = MoveStats.JumpBufferTime;
//            _jumpReleasedDuringBuffer = false;
//        }

//        // when jump is released
//        if (InputManager.JumpWasReleased)
//        {
//            if (_jumpBufferTimer > 0f)
//            {
//                _jumpReleasedDuringBuffer = true;
//            }

//            if (_isJumping && VerticalVelocity > 0f)
//            {
//                if (_isPastApexThreshold)
//                {
//                    _isPastApexThreshold = false;
//                    _isFastFalling = true;
//                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
//                    VerticalVelocity = 0f;
//                }
//                else
//                {
//                    _isFastFalling = true;
//                    _fastFallReleaseSpeed = VerticalVelocity;
//                }
//            }
//        }

//        // initiate jump with jump buffering and coyote time
//        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
//        {
//            InitiateJump(1);

//            if (_jumpReleasedDuringBuffer)
//            {
//                _isFastFalling = true;
//                _fastFallReleaseSpeed = VerticalVelocity;
//            }
//        }

//        // double jump
//        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
//        {
//            _isFastFalling = false;
//            InitiateJump(1);
//        }

//        // handle air jump after the coyote time has lapsed (take off an extra jump so the player does not get a bonus jump)
//        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
//        {
//            InitiateJump(2);
//            _isFastFalling = false;
//        }

//        // landing
//        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
//        {
//            _isJumping = false;
//            _isFalling = false;
//            _isFastFalling = false;
//            _fastFallTime = 0f;
//            _isPastApexThreshold = false;
//            _numberOfJumpsUsed = 0;

//            VerticalVelocity = Physics2D.gravity.y;
//        }
//    }

//    private void InitiateJump(int numberOfJumpsUsed)
//    {
//        if (!_isJumping)
//        {
//            _isJumping = true;
//        }

//        _jumpBufferTimer = 0f;
//        _numberOfJumpsUsed += numberOfJumpsUsed;
//        VerticalVelocity = MoveStats.InitialJumpVelocity;
//        animator.SetTrigger(AnimationStrings.jumpTrigger);
//    }

//    private void Jump()
//    {
//        // apply gravity while jumping
//        if (_isJumping)
//        {
//            // check for head bump
//            if (_bumpedHead)
//            {
//                _isFastFalling = true;
//            }

//            // gravity on ascending 
//            if (VerticalVelocity >= 0f)
//            {
//                // apex controls
//                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

//                if (_apexPoint > MoveStats.ApexThreshold)
//                {
//                    if (!_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = true;
//                        _timePastApexThreshold = 0f;
//                    }

//                    if (_isPastApexThreshold)
//                    {
//                        _timePastApexThreshold += Time.fixedDeltaTime;

//                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
//                        {
//                            VerticalVelocity = 0f;
//                        }
//                        else
//                        {
//                            VerticalVelocity = -0.01f;
//                        }
//                    }
//                }
//                // gravity on descending but not past apex threshold
//                else
//                {
//                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//                    if (_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = false;
//                    }
//                }
//            }

//            // gravity on descending
//            else if (!_isFastFalling)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }

//            else if (VerticalVelocity < 0f)
//            {
//                if (!_isFalling)
//                {
//                    _isFalling = true;
//                }
//            }
//        }

//        // jump cut
//        if (_isFastFalling)
//        {
//            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }
//            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
//            }

//            _fastFallTime += Time.fixedDeltaTime;
//        }

//        // normal gravity while falling
//        if (!_isGrounded && !_isJumping)
//        {
//            if (!_isFalling)
//            {
//                _isFalling = true;
//            }

//            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//        }

//        // clamp fall speed
//        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 80f);

//        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
//    }

//    #endregion

//    #region Collision Checks

//    private void IsGrounded()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

//        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

//        if (_groundHit.collider != null)
//        {
//            _isGrounded = true;
//        }
//        else { _isGrounded = false; }

//        #region Debug Visualizaton
//        if (MoveStats.DebugShowIsGroundedBox)
//        {
//            Color rayColor;
//            if (_isGrounded)
//            {
//                rayColor = Color.green;
//            }
//            else { rayColor = Color.red; }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
//        }
//        #endregion
//    }

//    private void BumpedHead()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

//        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

//        if (_headHit.collider != null)
//        {
//            _bumpedHead = true;
//        }
//        else { _bumpedHead = false; }

//        #region Debug Visualizaton 
//        if (MoveStats.DebugShowHeadBumpBox)
//        {
//            float headWidth = MoveStats.HeadWidth;

//            Color rayColor;
//            if (_bumpedHead)
//            {
//                rayColor = Color.green;
//            }
//            else
//            {
//                rayColor = Color.red;
//            }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
//        }

//        #endregion 
//    }

//    private void CollisionChecks()
//    {
//        IsGrounded();
//        BumpedHead();
//    }
//    #endregion

//    #region Timers

//    private void CountTimers()
//    {
//        _jumpBufferTimer -= Time.deltaTime;

//        if (!_isGrounded)
//        {
//            _coyoteTimer -= Time.deltaTime;
//        }
//        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
//    }

//    #endregion

//    #region On Trigger / On Collision

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // Check if the player touches a key
//        if (other.CompareTag("Key"))
//        {
//            Key key = other.GetComponent<Key>();

//            if (key != null)
//            {
//                Debug.Log("Player collected a key.");
//                Destroy(other.gameObject); // Remove the key
//            }
//        }


//        // Water hazard - player drowns
//        else if (other.gameObject.CompareTag("Water"))
//        {
//            // Deal fatal damage to drown the player
//            damageable.Hit(damageable.Health, Vector2.zero);
//        }
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("Mice"))
//        {
//            Mice mouse = other.GetComponent<Mice>();

//            if (mouse != null)
//            {
//                // Destroy the mouse GameObject immediately upon pickup
//                Destroy(mouse.gameObject);

//                // Toggle all platforms upon pickup
//                TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
//                foreach (TilemapToggle toggle in toggles)
//                {
//                    toggle.TogglePlatform();
//                }

//                Debug.Log("Mouse picked up and platforms toggled!");
//            }
//        }
//    }


//    #endregion

//    #region Toggle Platforms

//    private void ToggleAllPlatforms()
//    {
//        TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
//        foreach (TilemapToggle toggle in toggles)
//        {
//            toggle.TogglePlatform();
//        }
//    }
//    #endregion

//    #region Death / Hit

//    public void TriggerPlayerDeath()
//    {
//        PlayerDied?.Invoke();
//    }
//    public void OnHit(int damage, Vector2 knockback)
//    {
//        _rb.linearVelocity = new Vector2(knockback.x, _rb.linearVelocity.y + knockback.y);
//    }


//    #endregion
//}

// knockback
//using System;
//using System.Collections;
//using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

//public class PlayerMovement : MonoBehaviour
//{

//    [Header("References")]
//    public PlayerMovementStats MoveStats;
//    [SerializeField] private Collider2D _feetColl;
//    [SerializeField] private Collider2D _bodyColl;

//    private Rigidbody2D _rb;
//    private Animator animator;
//    private Damageable damageable;

//    // movement variables
//    private Vector2 _moveVelocity;
//    public bool _isFacingRight;

//    // collision check vars
//    private RaycastHit2D _groundHit;
//    private RaycastHit2D _headHit;
//    private bool _isGrounded;
//    private bool _bumpedHead;

//    // jump variables
//    public float VerticalVelocity { get; private set; }
//    private bool _isJumping;
//    private bool _isFastFalling;
//    private bool _isFalling;
//    private float _fastFallTime;
//    private float _fastFallReleaseSpeed;
//    private int _numberOfJumpsUsed;

//    // apex variables
//    private float _apexPoint;
//    private float _timePastApexThreshold;
//    private bool _isPastApexThreshold;

//    // jump buffer variables
//    private float _jumpBufferTimer;
//    private bool _jumpReleasedDuringBuffer;

//    // coyote time variables
//    private float _coyoteTimer;

//    // knockback variables
//    private bool isKnockbackActive = false;
//    private Vector2 _knockbackVelocity;


//    // for the fixed camera
//    private Vector3 initialPosition; // Stores the original spawn position
//    private bool positionCorrected = false; // Ensures we only correct position once

//    // key collection
//    public Key cm;
//    public event Action PlayerDied;


//    private void Awake()
//    {
//        _isFacingRight = true;

//        _rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        damageable = GetComponent<Damageable>();

//        // prevent movement at scene start - for fixed camera
//        _rb.bodyType = RigidbodyType2D.Kinematic;

//        // Ensure the knockback effect is applied when the player is hit
//        damageable.damageableHit.AddListener(OnHit);
//    }

//    private void FixedUpdate()
//    {
//        CollisionChecks();
//        Jump();

//        if (_isGrounded)
//        {
//            // If knockback is active, only apply vertical movement
//            if (isKnockbackActive)
//            {
//                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
//            }
//            else
//            {
//                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
//            }
//        }
//        else
//        {
//            if (isKnockbackActive)
//            {
//                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
//            }
//            else
//            {
//                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
//            }
//        }

//        // Animator updates
//        animator.SetBool(AnimationStrings.isMoving, InputManager.Movement.x != 0);
//        animator.SetBool(AnimationStrings.isGrounded, _isGrounded);
//        animator.SetFloat(AnimationStrings.yVelocity, _rb.linearVelocity.y);
//    }


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
//        CountTimers();
//        JumpChecks();
//    }

//    #region Camera
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

//        _rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
//    }

//    #endregion

//    #region Movement

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

//    // with run

//    //private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    //{
//    //    // Prevent movement if the player cannot move or is dead
//    //    if (!CanMove || !IsAlive)
//    //    {
//    //        _moveVelocity = Vector2.zero;
//    //        _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//    //        return;
//    //    }

//    //    if (moveInput != Vector2.zero)
//    //    {
//    //        TurnCheck(moveInput);

//    //        Vector2 targetVelocity = Vector2.zero;

//    //        if (InputManager.RunIsHeld)
//    //        {
//    //            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
//    //        }

//    //        else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

//    //        _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//    //    }

//    //    else if (moveInput == Vector2.zero)
//    //    {
//    //        _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//    //    }

//    //}

//    // without run

//    private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    {
//        // Prevent movement if the player cannot move or is dead
//        if (!CanMove || !IsAlive)
//        {
//            _moveVelocity = Vector2.zero;
//            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//            return;
//        }

//        if (moveInput != Vector2.zero)
//        {
//            TurnCheck(moveInput);

//            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;

//            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }
//        else
//        {
//            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//    }

//    private void TurnCheck(Vector2 moveInput)
//    {
//        bool shouldFaceRight = moveInput.x > 0;
//        bool shouldFaceLeft = moveInput.x < 0;

//        if (shouldFaceRight && !_isFacingRight)
//        {
//            Flip();
//        }
//        else if (shouldFaceLeft && _isFacingRight)
//        {
//            Flip();
//        }
//    }

//    // animator
//    private void Flip()
//    {
//        _isFacingRight = !_isFacingRight;
//        transform.localScale *= new Vector2(-1, 1);
//    }

//    private void Turn(bool turnRight)
//    {
//        if (turnRight)
//        {
//            _isFacingRight = true;
//            transform.Rotate(0f, 180f, 0f);
//        }
//        else
//        {
//            _isFacingRight = false;
//            transform.Rotate(0f, -180f, 0f);
//        }
//    }

//    #endregion

//    #region Jump
//    private void JumpChecks()
//    {

//        // Prevent jumping when the player is dead
//        if (!IsAlive)
//        {
//            return;
//        }

//        // when jump is pressed
//        if (InputManager.JumpWasPressed)
//        {
//            _jumpBufferTimer = MoveStats.JumpBufferTime;
//            _jumpReleasedDuringBuffer = false;
//        }

//        // when jump is released
//        if (InputManager.JumpWasReleased)
//        {
//            if (_jumpBufferTimer > 0f)
//            {
//                _jumpReleasedDuringBuffer = true;
//            }

//            if (_isJumping && VerticalVelocity > 0f)
//            {
//                if (_isPastApexThreshold)
//                {
//                    _isPastApexThreshold = false;
//                    _isFastFalling = true;
//                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
//                    VerticalVelocity = 0f;
//                }
//                else
//                {
//                    _isFastFalling = true;
//                    _fastFallReleaseSpeed = VerticalVelocity;
//                }
//            }
//        }

//        // initiate jump with jump buffering and coyote time
//        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
//        {
//            InitiateJump(1);

//            if (_jumpReleasedDuringBuffer)
//            {
//                _isFastFalling = true;
//                _fastFallReleaseSpeed = VerticalVelocity;
//            }
//        }

//        // double jump
//        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
//        {
//            _isFastFalling = false;
//            InitiateJump(1);
//        }

//        // handle air jump after the coyote time has lapsed (take off an extra jump so the player does not get a bonus jump)
//        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
//        {
//            InitiateJump(2);
//            _isFastFalling = false;
//        }

//        // landing
//        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
//        {
//            _isJumping = false;
//            _isFalling = false;
//            _isFastFalling = false;
//            _fastFallTime = 0f;
//            _isPastApexThreshold = false;
//            _numberOfJumpsUsed = 0;

//            VerticalVelocity = Physics2D.gravity.y;
//        }
//    }

//    private void InitiateJump(int numberOfJumpsUsed)
//    {
//        if (!_isJumping)
//        {
//            _isJumping = true;
//        }

//        _jumpBufferTimer = 0f;
//        _numberOfJumpsUsed += numberOfJumpsUsed;
//        VerticalVelocity = MoveStats.InitialJumpVelocity;
//        animator.SetTrigger(AnimationStrings.jumpTrigger);
//    }

//    private void Jump()
//    {
//        // apply gravity while jumping
//        if (_isJumping)
//        {
//            // check for head bump
//            if (_bumpedHead)
//            {
//                _isFastFalling = true;
//            }

//            // gravity on ascending 
//            if (VerticalVelocity >= 0f)
//            {
//                // apex controls
//                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

//                if (_apexPoint > MoveStats.ApexThreshold)
//                {
//                    if (!_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = true;
//                        _timePastApexThreshold = 0f;
//                    }

//                    if (_isPastApexThreshold)
//                    {
//                        _timePastApexThreshold += Time.fixedDeltaTime;

//                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
//                        {
//                            VerticalVelocity = 0f;
//                        }
//                        else
//                        {
//                            VerticalVelocity = -0.01f;
//                        }
//                    }
//                }
//                // gravity on descending but not past apex threshold
//                else
//                {
//                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//                    if (_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = false;
//                    }
//                }
//            }

//            // gravity on descending
//            else if (!_isFastFalling)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }

//            else if (VerticalVelocity < 0f)
//            {
//                if (!_isFalling)
//                {
//                    _isFalling = true;
//                }
//            }
//        }

//        // jump cut
//        if (_isFastFalling)
//        {
//            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }
//            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
//            }

//            _fastFallTime += Time.fixedDeltaTime;
//        }

//        // normal gravity while falling
//        if (!_isGrounded && !_isJumping)
//        {
//            if (!_isFalling)
//            {
//                _isFalling = true;
//            }

//            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//        }

//        // clamp fall speed
//        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 80f);

//        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
//    }

//    #endregion

//    #region Collision Checks

//    private void IsGrounded()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

//        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

//        if (_groundHit.collider != null)
//        {
//            _isGrounded = true;
//        }
//        else { _isGrounded = false; }

//        #region Debug Visualizaton
//        if (MoveStats.DebugShowIsGroundedBox)
//        {
//            Color rayColor;
//            if (_isGrounded)
//            {
//                rayColor = Color.green;
//            }
//            else { rayColor = Color.red; }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
//        }
//        #endregion
//    }

//    private void BumpedHead()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

//        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

//        if (_headHit.collider != null)
//        {
//            _bumpedHead = true;
//        }
//        else { _bumpedHead = false; }

//        #region Debug Visualizaton 
//        if (MoveStats.DebugShowHeadBumpBox)
//        {
//            float headWidth = MoveStats.HeadWidth;

//            Color rayColor;
//            if (_bumpedHead)
//            {
//                rayColor = Color.green;
//            }
//            else
//            {
//                rayColor = Color.red;
//            }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
//        }

//        #endregion 
//    }

//    private void CollisionChecks()
//    {
//        IsGrounded();
//        BumpedHead();
//    }
//    #endregion

//    #region Timers

//    private void CountTimers()
//    {
//        _jumpBufferTimer -= Time.deltaTime;

//        if (!_isGrounded)
//        {
//            _coyoteTimer -= Time.deltaTime;
//        }
//        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
//    }

//    #endregion

//    #region On Trigger / On Collision

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // Check if the player touches a key
//        if (other.CompareTag("Key"))
//        {
//            Key key = other.GetComponent<Key>();

//            if (key != null)
//            {
//                Debug.Log("Player collected a key.");
//                Destroy(other.gameObject); // Remove the key
//            }
//        }


//        // Water hazard - player drowns
//        else if (other.gameObject.CompareTag("Water"))
//        {
//            // Deal fatal damage to drown the player
//            damageable.Hit(damageable.Health, Vector2.zero);
//        }
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("Mice"))
//        {
//            Mice mouse = other.GetComponent<Mice>();

//            if (mouse != null)
//            {
//                // Destroy the mouse GameObject immediately upon pickup
//                Destroy(mouse.gameObject);

//                // Toggle all platforms upon pickup
//                TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
//                foreach (TilemapToggle toggle in toggles)
//                {
//                    toggle.TogglePlatform();
//                }

//                Debug.Log("Mouse picked up and platforms toggled!");
//            }
//        }
//    }


//    #endregion

//    #region Toggle Platforms

//    private void ToggleAllPlatforms()
//    {
//        TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
//        foreach (TilemapToggle toggle in toggles)
//        {
//            toggle.TogglePlatform();
//        }
//    }
//    #endregion

//    #region Death / Hit / Knockback

//    public void TriggerPlayerDeath()
//    {
//        PlayerDied?.Invoke();
//    }
//    public void OnHit(int damage, Vector2 knockback)
//    {
//        Debug.Log($"OnHit called! Applying knockback: {knockback}");

//        StopAllCoroutines(); // Prevent stacking knockbacks
//        StartCoroutine(ApplyKnockback(knockback));
//    }

//    private IEnumerator ApplyKnockback(Vector2 knockback)
//    {
//        float knockbackDuration = 0.2f; // Adjust duration as needed
//        isKnockbackActive = true;
//        _knockbackVelocity = knockback;

//        Debug.Log($"Knockback started: {_knockbackVelocity}");

//        yield return new WaitForSeconds(knockbackDuration);

//        isKnockbackActive = false;
//        _knockbackVelocity = Vector2.zero;

//        Debug.Log("Knockback ended");
//    }

//    #endregion
//}


// blue and red mice
//using System;
//using System.Collections;
//using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

//public class PlayerMovement : MonoBehaviour
//{

//    [Header("References")]
//    public PlayerMovementStats MoveStats;
//    [SerializeField] private Collider2D _feetColl;
//    [SerializeField] private Collider2D _bodyColl;

//    private Rigidbody2D _rb;
//    private Animator animator;
//    private Damageable damageable;

//    // movement variables
//    private Vector2 _moveVelocity;
//    public bool _isFacingRight;

//    // collision check vars
//    private RaycastHit2D _groundHit;
//    private RaycastHit2D _headHit;
//    private bool _isGrounded;
//    private bool _bumpedHead;

//    // jump variables
//    public float VerticalVelocity { get; private set; }
//    private bool _isJumping;
//    private bool _isFastFalling;
//    private bool _isFalling;
//    private float _fastFallTime;
//    private float _fastFallReleaseSpeed;
//    private int _numberOfJumpsUsed;

//    // apex variables
//    private float _apexPoint;
//    private float _timePastApexThreshold;
//    private bool _isPastApexThreshold;

//    // jump buffer variables
//    private float _jumpBufferTimer;
//    private bool _jumpReleasedDuringBuffer;

//    // coyote time variables
//    private float _coyoteTimer;

//    // knockback variables
//    private bool isKnockbackActive = false;
//    private Vector2 _knockbackVelocity;


//    // for the fixed camera
//    private Vector3 initialPosition; // Stores the original spawn position
//    private bool positionCorrected = false; // Ensures we only correct position once

//    // key collection
//    public Key cm;
//    public event Action PlayerDied;

//    // mouse
//    private Mice currentMouse = null;

//    private void Awake()
//    {
//        _isFacingRight = true;

//        _rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        damageable = GetComponent<Damageable>();

//        // prevent movement at scene start - for fixed camera
//        _rb.bodyType = RigidbodyType2D.Kinematic;

//        // Ensure the knockback effect is applied when the player is hit
//        damageable.damageableHit.AddListener(OnHit);
//    }

//    private void FixedUpdate()
//    {
//        CollisionChecks();
//        Jump();

//        if (_isGrounded)
//        {
//            // If knockback is active, only apply vertical movement
//            if (isKnockbackActive)
//            {
//                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
//            }
//            else
//            {
//                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
//            }
//        }
//        else
//        {
//            if (isKnockbackActive)
//            {
//                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
//            }
//            else
//            {
//                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
//            }
//        }

//        // Animator updates
//        animator.SetBool(AnimationStrings.isMoving, InputManager.Movement.x != 0);
//        animator.SetBool(AnimationStrings.isGrounded, _isGrounded);
//        animator.SetFloat(AnimationStrings.yVelocity, _rb.linearVelocity.y);
//    }


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
//        CountTimers();
//        JumpChecks();
//    }

//    #region Camera
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

//        _rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
//    }

//    #endregion

//    #region Movement

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

//    // with run

//    //private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    //{
//    //    // Prevent movement if the player cannot move or is dead
//    //    if (!CanMove || !IsAlive)
//    //    {
//    //        _moveVelocity = Vector2.zero;
//    //        _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//    //        return;
//    //    }

//    //    if (moveInput != Vector2.zero)
//    //    {
//    //        TurnCheck(moveInput);

//    //        Vector2 targetVelocity = Vector2.zero;

//    //        if (InputManager.RunIsHeld)
//    //        {
//    //            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
//    //        }

//    //        else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

//    //        _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//    //    }

//    //    else if (moveInput == Vector2.zero)
//    //    {
//    //        _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//    //    }

//    //}

//    // without run

//    private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    {
//        // Prevent movement if the player cannot move or is dead
//        if (!CanMove || !IsAlive)
//        {
//            _moveVelocity = Vector2.zero;
//            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//            return;
//        }

//        if (moveInput != Vector2.zero)
//        {
//            TurnCheck(moveInput);

//            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;

//            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }
//        else
//        {
//            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//    }

//    public bool IsFacingRight()
//    {
//        return _isFacingRight;
//    }

//    private void TurnCheck(Vector2 moveInput)
//    {
//        bool shouldFaceRight = moveInput.x > 0;
//        bool shouldFaceLeft = moveInput.x < 0;

//        if (shouldFaceRight && !_isFacingRight)
//        {
//            Flip();
//        }
//        else if (shouldFaceLeft && _isFacingRight)
//        {
//            Flip();
//        }
//    }

//    // animator
//    private void Flip()
//    {
//        _isFacingRight = !_isFacingRight;
//        transform.localScale *= new Vector2(-1, 1);
//    }


//    private void Turn(bool turnRight)
//    {
//        if (turnRight)
//        {
//            _isFacingRight = true;
//            transform.Rotate(0f, 180f, 0f);
//        }
//        else
//        {
//            _isFacingRight = false;
//            transform.Rotate(0f, -180f, 0f);
//        }
//    }

//    #endregion

//    #region Jump
//    private void JumpChecks()
//    {

//        // Prevent jumping when the player is dead
//        if (!IsAlive)
//        {
//            return;
//        }

//        // when jump is pressed
//        if (InputManager.JumpWasPressed)
//        {
//            _jumpBufferTimer = MoveStats.JumpBufferTime;
//            _jumpReleasedDuringBuffer = false;
//        }

//        // when jump is released
//        if (InputManager.JumpWasReleased)
//        {
//            if (_jumpBufferTimer > 0f)
//            {
//                _jumpReleasedDuringBuffer = true;
//            }

//            if (_isJumping && VerticalVelocity > 0f)
//            {
//                if (_isPastApexThreshold)
//                {
//                    _isPastApexThreshold = false;
//                    _isFastFalling = true;
//                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
//                    VerticalVelocity = 0f;
//                }
//                else
//                {
//                    _isFastFalling = true;
//                    _fastFallReleaseSpeed = VerticalVelocity;
//                }
//            }
//        }

//        // initiate jump with jump buffering and coyote time
//        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
//        {
//            InitiateJump(1);

//            if (_jumpReleasedDuringBuffer)
//            {
//                _isFastFalling = true;
//                _fastFallReleaseSpeed = VerticalVelocity;
//            }
//        }

//        // double jump
//        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
//        {
//            _isFastFalling = false;
//            InitiateJump(1);
//        }

//        // handle air jump after the coyote time has lapsed (take off an extra jump so the player does not get a bonus jump)
//        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
//        {
//            InitiateJump(2);
//            _isFastFalling = false;
//        }

//        // landing
//        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
//        {
//            _isJumping = false;
//            _isFalling = false;
//            _isFastFalling = false;
//            _fastFallTime = 0f;
//            _isPastApexThreshold = false;
//            _numberOfJumpsUsed = 0;

//            VerticalVelocity = Physics2D.gravity.y;
//        }
//    }

//    private void InitiateJump(int numberOfJumpsUsed)
//    {
//        if (!_isJumping)
//        {
//            _isJumping = true;
//        }

//        _jumpBufferTimer = 0f;
//        _numberOfJumpsUsed += numberOfJumpsUsed;
//        VerticalVelocity = MoveStats.InitialJumpVelocity;
//        animator.SetTrigger(AnimationStrings.jumpTrigger);
//    }

//    private void Jump()
//    {
//        // apply gravity while jumping
//        if (_isJumping)
//        {
//            // check for head bump
//            if (_bumpedHead)
//            {
//                _isFastFalling = true;
//            }

//            // gravity on ascending 
//            if (VerticalVelocity >= 0f)
//            {
//                // apex controls
//                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

//                if (_apexPoint > MoveStats.ApexThreshold)
//                {
//                    if (!_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = true;
//                        _timePastApexThreshold = 0f;
//                    }

//                    if (_isPastApexThreshold)
//                    {
//                        _timePastApexThreshold += Time.fixedDeltaTime;

//                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
//                        {
//                            VerticalVelocity = 0f;
//                        }
//                        else
//                        {
//                            VerticalVelocity = -0.01f;
//                        }
//                    }
//                }
//                // gravity on descending but not past apex threshold
//                else
//                {
//                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//                    if (_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = false;
//                    }
//                }
//            }

//            // gravity on descending
//            else if (!_isFastFalling)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }

//            else if (VerticalVelocity < 0f)
//            {
//                if (!_isFalling)
//                {
//                    _isFalling = true;
//                }
//            }
//        }

//        // jump cut
//        if (_isFastFalling)
//        {
//            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }
//            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
//            }

//            _fastFallTime += Time.fixedDeltaTime;
//        }

//        // normal gravity while falling
//        if (!_isGrounded && !_isJumping)
//        {
//            if (!_isFalling)
//            {
//                _isFalling = true;
//            }

//            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//        }

//        // clamp fall speed
//        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 80f);

//        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
//    }

//    #endregion

//    #region Collision Checks

//    private void IsGrounded()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

//        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

//        if (_groundHit.collider != null)
//        {
//            _isGrounded = true;
//        }
//        else { _isGrounded = false; }

//        #region Debug Visualizaton
//        if (MoveStats.DebugShowIsGroundedBox)
//        {
//            Color rayColor;
//            if (_isGrounded)
//            {
//                rayColor = Color.green;
//            }
//            else { rayColor = Color.red; }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
//        }
//        #endregion
//    }

//    private void BumpedHead()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

//        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

//        if (_headHit.collider != null)
//        {
//            _bumpedHead = true;
//        }
//        else { _bumpedHead = false; }

//        #region Debug Visualizaton 
//        if (MoveStats.DebugShowHeadBumpBox)
//        {
//            float headWidth = MoveStats.HeadWidth;

//            Color rayColor;
//            if (_bumpedHead)
//            {
//                rayColor = Color.green;
//            }
//            else
//            {
//                rayColor = Color.red;
//            }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
//        }

//        #endregion 
//    }

//    private void CollisionChecks()
//    {
//        IsGrounded();
//        BumpedHead();
//    }
//    #endregion

//    #region Timers

//    private void CountTimers()
//    {
//        _jumpBufferTimer -= Time.deltaTime;

//        if (!_isGrounded)
//        {
//            _coyoteTimer -= Time.deltaTime;
//        }
//        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
//    }

//    #endregion

//    #region On Trigger / On Collision

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // Check if the player touches a key
//        if (other.CompareTag("Key"))
//        {
//            Key key = other.GetComponent<Key>();

//            if (key != null)
//            {
//                Debug.Log("Player collected a key.");
//                Destroy(other.gameObject); // Remove the key
//            }
//        }


//        // Water hazard - player drowns
//        else if (other.gameObject.CompareTag("Water"))
//        {
//            // Deal fatal damage to drown the player
//            damageable.Hit(damageable.Health, Vector2.zero);
//        }
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("Mice") && other.gameObject.layer == LayerMask.NameToLayer("PickupTrigger") && InputManager.PickupWasPressed)
//        {
//            Mice mouse = other.GetComponent<Mice>();
//            if (mouse != null)
//            {
//                PickupMouse(mouse);
//            }
//        }
//    }



//    #endregion

//    #region Mouse
//    //public void PickupMouse(Mice newMouse)
//    //{
//    //    if (currentMouse != null)
//    //    {
//    //        currentMouse.DropMouse();
//    //    }

//    //    currentMouse = newMouse;
//    //    currentMouse.SetFollowingPlayer(transform);

//    //    ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);

//    //    Debug.Log("Mouse successfully picked up!");
//    //}

//    public void PickupMouse(Mice newMouse)
//    {
//        if (currentMouse != null)
//        {
//            // Drop the previous mouse at the designated drop position
//            Vector3 dropPosition = transform.position; // Change this if you want a specific drop point
//            currentMouse.DropMouse(dropPosition);
//        }

//        // Assign the new mouse and place it on the player's head
//        currentMouse = newMouse;
//        currentMouse.SetOnPlayerHead(transform); // FIXED! Uses the correct function

//        // Toggle platforms based on the new mouse's type
//        ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);
//    }



//    private void ToggleCorrespondingPlatforms(bool isBlue)
//    {
//        PlatformToggle[] toggles = FindObjectsByType<PlatformToggle>(FindObjectsSortMode.None);

//        foreach (PlatformToggle toggle in toggles)
//        {
//            if (toggle.isBluePlatform)
//            {
//                toggle.SetPlatformState(isBlue); // Activate blue platforms if picking up blue mouse
//            }
//            else
//            {
//                toggle.SetPlatformState(!isBlue); // Activate red platforms if picking up red mouse
//            }
//        }
//    }

//    #endregion

//    #region Death / Hit / Knockback / Checkpoint


//    public void TriggerPlayerDeath()
//    {
//        PlayerDied?.Invoke();
//    }

//    public void OnHit(int damage, Vector2 knockback)
//    {
//        Debug.Log($"OnHit called! Applying knockback: {knockback}");

//        StopAllCoroutines(); // Prevent stacking knockbacks
//        StartCoroutine(ApplyKnockback(knockback));
//    }

//    private IEnumerator ApplyKnockback(Vector2 knockback)
//    {
//        float knockbackDuration = 0.2f; // Adjust duration as needed
//        isKnockbackActive = true;
//        _knockbackVelocity = knockback;

//        Debug.Log($"Knockback started: {_knockbackVelocity}");

//        yield return new WaitForSeconds(knockbackDuration);

//        isKnockbackActive = false;
//        _knockbackVelocity = Vector2.zero;

//        Debug.Log("Knockback ended");
//    }

//    #endregion
//}


// push off mouse version
//using System;
//using System.Collections;
//using UnityEngine;
//// using static UnityEngine.Rendering.DebugUI;

//public class PlayerMovement : MonoBehaviour
//{

//    [Header("References")]
//    public PlayerMovementStats MoveStats;
//    [SerializeField] private Collider2D _feetColl;
//    [SerializeField] private Collider2D _bodyColl;

//    private Rigidbody2D _rb;
//    private Animator animator;
//    private Damageable damageable;

//    // movement variables
//    private Vector2 _moveVelocity;
//    public bool _isFacingRight;

//    // collision check vars
//    private RaycastHit2D _groundHit;
//    private RaycastHit2D _headHit;
//    private bool _isGrounded;
//    private bool _bumpedHead;

//    // jump variables
//    public float VerticalVelocity { get; private set; }
//    private bool _isJumping;
//    private bool _isFastFalling;
//    private bool _isFalling;
//    private float _fastFallTime;
//    private float _fastFallReleaseSpeed;
//    private int _numberOfJumpsUsed;

//    // apex variables
//    private float _apexPoint;
//    private float _timePastApexThreshold;
//    private bool _isPastApexThreshold;

//    // jump buffer variables
//    private float _jumpBufferTimer;
//    private bool _jumpReleasedDuringBuffer;

//    // coyote time variables
//    private float _coyoteTimer;

//    // knockback variables
//    private bool isKnockbackActive = false;
//    private Vector2 _knockbackVelocity;


//    // for the fixed camera
//    private Vector3 initialPosition; // Stores the original spawn position
//    private bool positionCorrected = false; // Ensures we only correct position once

//    // key collection
//    public Key cm;
//    public event Action PlayerDied;

//    // mouse
//    private Mice currentMouse = null;

//    AudioManager audioManager;


//    private void Awake()
//    {
//        _isFacingRight = true;

//        _rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        damageable = GetComponent<Damageable>();

//        // prevent movement at scene start - for fixed camera
//        _rb.bodyType = RigidbodyType2D.Kinematic;

//        // Ensure the knockback effect is applied when the player is hit
//        damageable.damageableHit.AddListener(OnHit);

//        //audioSFX
//        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
//    }


//    private void FixedUpdate()
//    {
//        CollisionChecks();
//        Jump();

//        if (_isGrounded)
//        {
//            // If knockback is active, only apply vertical movement
//            if (isKnockbackActive)
//            {
//                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
//            }
//            else
//            {
//                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
//            }
//        }
//        else
//        {
//            if (isKnockbackActive)
//            {
//                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
//            }
//            else
//            {
//                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
//            }
//        }

//        // Animator updates
//        animator.SetBool(AnimationStrings.isMoving, InputManager.Movement.x != 0);
//        animator.SetBool(AnimationStrings.isGrounded, _isGrounded);
//        animator.SetFloat(AnimationStrings.yVelocity, _rb.linearVelocity.y);
//    }


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
//        CountTimers();
//        JumpChecks();
//    }

//    #region Camera
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

//        _rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
//    }

//    #endregion

//    #region Movement

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

//    // with run

//    //private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    //{
//    //    // Prevent movement if the player cannot move or is dead
//    //    if (!CanMove || !IsAlive)
//    //    {
//    //        _moveVelocity = Vector2.zero;
//    //        _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//    //        return;
//    //    }

//    //    if (moveInput != Vector2.zero)
//    //    {
//    //        TurnCheck(moveInput);

//    //        Vector2 targetVelocity = Vector2.zero;

//    //        if (InputManager.RunIsHeld)
//    //        {
//    //            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
//    //        }

//    //        else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

//    //        _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//    //    }

//    //    else if (moveInput == Vector2.zero)
//    //    {
//    //        _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//    //    }

//    //}

//    // without run

//    private void Move(float acceleration, float deceleration, Vector2 moveInput)
//    {
//        // Prevent movement if the player cannot move or is dead
//        if (!CanMove || !IsAlive)
//        {
//            _moveVelocity = Vector2.zero;
//            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
//            return;
//        }

//        if (moveInput != Vector2.zero)
//        {
//            TurnCheck(moveInput);

//            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;

//            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }
//        else
//        {
//            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//        }

//    }

//    public bool IsFacingRight()
//    {
//        return _isFacingRight;
//    }

//    private void TurnCheck(Vector2 moveInput)
//    {
//        bool shouldFaceRight = moveInput.x > 0;
//        bool shouldFaceLeft = moveInput.x < 0;

//        if (shouldFaceRight && !_isFacingRight)
//        {
//            Flip();
//        }
//        else if (shouldFaceLeft && _isFacingRight)
//        {
//            Flip();
//        }
//    }

//    // animator
//    private void Flip()
//    {
//        _isFacingRight = !_isFacingRight;
//        transform.localScale *= new Vector2(-1, 1);
//    }


//    private void Turn(bool turnRight)
//    {
//        if (turnRight)
//        {
//            _isFacingRight = true;
//            transform.Rotate(0f, 180f, 0f);
//        }
//        else
//        {
//            _isFacingRight = false;
//            transform.Rotate(0f, -180f, 0f);
//        }
//    }

//    #endregion

//    #region Jump
//    private void JumpChecks()
//    {

//        // Prevent jumping when the player is dead
//        if (!IsAlive)
//        {
//            return;
//        }

//        // when jump is pressed
//        if (InputManager.JumpWasPressed)
//        {
//            _jumpBufferTimer = MoveStats.JumpBufferTime;
//            _jumpReleasedDuringBuffer = false;
//            audioManager.PlaySFX(audioManager.jump); //audio sfx
//        }

//        // when jump is released
//        if (InputManager.JumpWasReleased)
//        {
//            if (_jumpBufferTimer > 0f)
//            {
//                _jumpReleasedDuringBuffer = true;
//            }

//            if (_isJumping && VerticalVelocity > 0f)
//            {
//                if (_isPastApexThreshold)
//                {
//                    _isPastApexThreshold = false;
//                    _isFastFalling = true;
//                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
//                    VerticalVelocity = 0f;
//                }
//                else
//                {
//                    _isFastFalling = true;
//                    _fastFallReleaseSpeed = VerticalVelocity;
//                }
//            }
//        }

//        // initiate jump with jump buffering and coyote time
//        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
//        {
//            InitiateJump(1);

//            if (_jumpReleasedDuringBuffer)
//            {
//                _isFastFalling = true;
//                _fastFallReleaseSpeed = VerticalVelocity;
//            }
//        }

//        // double jump
//        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
//        {
//            _isFastFalling = false;
//            InitiateJump(1);
//        }

//        // handle air jump after the coyote time has lapsed (take off an extra jump so the player does not get a bonus jump)
//        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
//        {
//            InitiateJump(2);
//            _isFastFalling = false;
//        }

//        // landing
//        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
//        {
//            _isJumping = false;
//            _isFalling = false;
//            _isFastFalling = false;
//            _fastFallTime = 0f;
//            _isPastApexThreshold = false;
//            _numberOfJumpsUsed = 0;

//            VerticalVelocity = Physics2D.gravity.y;
//        }
//    }

//    private void InitiateJump(int numberOfJumpsUsed)
//    {
//        if (!_isJumping)
//        {
//            _isJumping = true;
//        }

//        _jumpBufferTimer = 0f;
//        _numberOfJumpsUsed += numberOfJumpsUsed;
//        VerticalVelocity = MoveStats.InitialJumpVelocity;
//        animator.SetTrigger(AnimationStrings.jumpTrigger);
//    }

//    private void Jump()
//    {
//        // apply gravity while jumping
//        if (_isJumping)
//        {
//            // check for head bump
//            if (_bumpedHead)
//            {
//                _isFastFalling = true;
//            }

//            // gravity on ascending 
//            if (VerticalVelocity >= 0f)
//            {
//                // apex controls
//                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

//                if (_apexPoint > MoveStats.ApexThreshold)
//                {
//                    if (!_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = true;
//                        _timePastApexThreshold = 0f;
//                    }

//                    if (_isPastApexThreshold)
//                    {
//                        _timePastApexThreshold += Time.fixedDeltaTime;

//                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
//                        {
//                            VerticalVelocity = 0f;
//                        }
//                        else
//                        {
//                            VerticalVelocity = -0.01f;
//                        }
//                    }
//                }
//                // gravity on descending but not past apex threshold
//                else
//                {
//                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//                    if (_isPastApexThreshold)
//                    {
//                        _isPastApexThreshold = false;
//                    }
//                }
//            }

//            // gravity on descending
//            else if (!_isFastFalling)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }

//            else if (VerticalVelocity < 0f)
//            {
//                if (!_isFalling)
//                {
//                    _isFalling = true;
//                }
//            }
//        }

//        // jump cut
//        if (_isFastFalling)
//        {
//            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//            }
//            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
//            {
//                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
//            }

//            _fastFallTime += Time.fixedDeltaTime;
//        }

//        // normal gravity while falling
//        if (!_isGrounded && !_isJumping)
//        {
//            if (!_isFalling)
//            {
//                _isFalling = true;
//            }

//            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//        }

//        // clamp fall speed
//        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 80f);

//        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
//    }

//    #endregion

//    #region Collision Checks

//    private void IsGrounded()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

//        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

//        if (_groundHit.collider != null)
//        {
//            _isGrounded = true;

//        }
//        else { _isGrounded = false; }

//        #region Debug Visualizaton
//        if (MoveStats.DebugShowIsGroundedBox)
//        {
//            Color rayColor;
//            if (_isGrounded)
//            {
//                rayColor = Color.green;
//            }
//            else { rayColor = Color.red; }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
//        }
//        #endregion
//    }

//    private void BumpedHead()
//    {
//        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
//        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

//        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

//        if (_headHit.collider != null)
//        {
//            _bumpedHead = true;
//            audioManager.PlaySFX(audioManager.wallTouch); //audio sfx 

//        }
//        else { _bumpedHead = false; }

//        #region Debug Visualizaton 
//        if (MoveStats.DebugShowHeadBumpBox)
//        {
//            float headWidth = MoveStats.HeadWidth;

//            Color rayColor;
//            if (_bumpedHead)
//            {
//                rayColor = Color.green;
//            }
//            else
//            {
//                rayColor = Color.red;
//            }

//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
//        }

//        #endregion 
//    }

//    private void CollisionChecks()
//    {
//        IsGrounded();
//        BumpedHead();
//    }
//    #endregion

//    #region Timers

//    private void CountTimers()
//    {
//        _jumpBufferTimer -= Time.deltaTime;

//        if (!_isGrounded)
//        {
//            _coyoteTimer -= Time.deltaTime;
//        }
//        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
//    }

//    #endregion

//    #region On Trigger / On Collision

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // Check if the player touches a key
//        if (other.CompareTag("Key"))
//        {
//            Key key = other.GetComponent<Key>();

//            if (key != null)
//            {
//                Debug.Log("Player collected a key.");
//                audioManager.PlaySFX(audioManager.keyPickup); //audio sfx
//                Destroy(other.gameObject); // Remove the key
//            }
//        }


//        // Water hazard - player drowns
//        else if (other.gameObject.CompareTag("Water"))
//        {
//            // Deal fatal damage to drown the player
//            damageable.Hit(damageable.Health, Vector2.zero);
//            audioManager.PlaySFX(audioManager.death); //audio sfx

//        }
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("Mice") && other.gameObject.layer == LayerMask.NameToLayer("PickupTrigger") && InputManager.PickupWasPressed)
//        {
//            Mice mouse = other.GetComponent<Mice>();
//            if (mouse != null)
//            {
//                PickupMouse(mouse);
//            }
//        }
//    }



//    #endregion

//    #region Mouse
//    //public void PickupMouse(Mice newMouse)
//    //{
//    //    if (currentMouse != null)
//    //    {
//    //        currentMouse.DropMouse();
//    //    }

//    //    currentMouse = newMouse;
//    //    currentMouse.SetFollowingPlayer(transform);

//    //    ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);

//    //    Debug.Log("Mouse successfully picked up!");
//    //}

//    public void PickupMouse(Mice newMouse)
//    {
//        if (currentMouse != null)
//        {
//            // Drop the previous mouse at the designated drop position
//            Vector3 dropPosition = transform.position; // Change this if you want a specific drop point
//            currentMouse.DropMouse(dropPosition);
//        }

//        // Assign the new mouse and place it on the player's head
//        currentMouse = newMouse;
//        currentMouse.SetOnPlayerHead(transform); // FIXED! Uses the correct function

//        // Toggle platforms based on the new mouse's type
//        ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);
//    }



//    private void ToggleCorrespondingPlatforms(bool isBlue)
//    {
//        PlatformToggle[] toggles = FindObjectsByType<PlatformToggle>(FindObjectsSortMode.None);

//        foreach (PlatformToggle toggle in toggles)
//        {
//            if (toggle.isBluePlatform)
//            {
//                toggle.SetPlatformState(isBlue); // Activate blue platforms if picking up blue mouse
//            }
//            else
//            {
//                toggle.SetPlatformState(!isBlue); // Activate red platforms if picking up red mouse
//            }
//        }
//    }

//    public bool HasMouse()
//    {
//        return currentMouse != null;
//    }

//    public Mice GetCurrentMouse()
//    {
//        return currentMouse;
//    }

//    public void RemoveMouse()
//    {
//        currentMouse = null;
//    }




//    #endregion

//    #region Death / Hit / Knockback / Checkpoint


//    //public void TriggerPlayerDeath()
//    //{
//    //    PlayerDied?.Invoke();
//    //}

//    public void TriggerPlayerDeath()
//    {
//        audioManager.PlaySFX(audioManager.death); //audio sfx
//        PlayerDied?.Invoke();

//        // Lock player movement by freezing Rigidbody
//        _rb.linearVelocity = Vector2.zero;
//        _rb.constraints = RigidbodyConstraints2D.FreezeAll; // Completely locks position & rotation

//        // Notify all enemies to stop attacking
//        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
//        foreach (Enemy enemy in enemies)
//        {
//            enemy.StopTargetingPlayer();
//        }

//        // If the player has a mouse, fade it out and remove it
//        if (currentMouse != null)
//        {
//            currentMouse.FadeAndDestroy();
//            currentMouse = null; // Clear reference
//        }

//    }

//    //public void RespawnPlayer()
//    //{
//    //    // Notify all enemies that the player is back
//    //    Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
//    //    foreach (Enemy enemy in enemies)
//    //    {
//    //        enemy.ResetEnemyTargeting();
//    //    }

//    //    Debug.Log("[DEBUG] Player respawned, enemies can attack again.");
//    //}



//    public void OnHit(int damage, Vector2 knockback)
//    {
//        Debug.Log($"OnHit called! Applying knockback: {knockback}");

//        StopAllCoroutines(); // Prevent stacking knockbacks
//        StartCoroutine(ApplyKnockback(knockback));
//    }

//    private IEnumerator ApplyKnockback(Vector2 knockback)
//    {
//        float knockbackDuration = 0.2f; // Adjust duration as needed
//        isKnockbackActive = true;
//        _knockbackVelocity = knockback;

//        Debug.Log($"Knockback started: {_knockbackVelocity}");

//        yield return new WaitForSeconds(knockbackDuration);

//        isKnockbackActive = false;
//        _knockbackVelocity = Vector2.zero;

//        Debug.Log("Knockback ended");
//    }

//    #endregion
//}


// checkpoint
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
// using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;
    private Animator animator;
    private Damageable damageable;

    // movement variables
    private Vector2 _moveVelocity;
    public bool _isFacingRight;

    // collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    // jump variables
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    // apex variables
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // jump buffer variables
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    // coyote time variables
    private float _coyoteTimer;

    // knockback variables
    private bool isKnockbackActive = false;
    private Vector2 _knockbackVelocity;


    // for the fixed camera
    private Vector3 initialPosition; // Stores the original spawn position
    private bool positionCorrected = false; // Ensures we only correct position once

    // key collection
    public Key keyPrefab;

    // player death
    public event Action PlayerDied;

    // mouse
    private Mice currentMouse = null;

    [SerializeField] public GameObject blueMousePrefab;
    [SerializeField] public GameObject redMousePrefab;

    private bool savedHasMouse = false;
    private bool savedMouseIsBlue = false;

    // game audio
    AudioManager audioManager;
    private AudioSource footstepsSource;



    private void Awake()
    {
        _isFacingRight = true;

        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        // prevent movement at scene start - for fixed camera
        _rb.bodyType = RigidbodyType2D.Kinematic;

        // Ensure the knockback effect is applied when the player is hit
        damageable.damageableHit.AddListener(OnHit);

        //audioSFX
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // walking audio
        footstepsSource = gameObject.AddComponent<AudioSource>();
        footstepsSource.clip = audioManager.walk;
        footstepsSource.loop = true;
        footstepsSource.playOnAwake = false;

    }


    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (_isGrounded)
        {
            // If knockback is active, only apply vertical movement
            if (isKnockbackActive)
            {
                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
            }
            else
            {
                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
            }
        }
        else
        {
            if (isKnockbackActive)
            {
                _rb.linearVelocity = new Vector2(_knockbackVelocity.x, _rb.linearVelocity.y);
            }
            else
            {
                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
            }
        }

        // Animator updates
        animator.SetBool(AnimationStrings.isMoving, InputManager.Movement.x != 0);
        animator.SetBool(AnimationStrings.isGrounded, _isGrounded);
        animator.SetFloat(AnimationStrings.yVelocity, _rb.linearVelocity.y);

        // footsteps audio
        HandleFootsteps();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        CountTimers();
        JumpChecks();
    }

    #region Camera
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

        _rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics after locking position
    }

    #endregion

    #region Movement

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


    public bool IsAlive { get; private set; } = true;


    // with run

    //private void Move(float acceleration, float deceleration, Vector2 moveInput)
    //{
    //    // Prevent movement if the player cannot move or is dead
    //    if (!CanMove || !IsAlive)
    //    {
    //        _moveVelocity = Vector2.zero;
    //        _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
    //        return;
    //    }

    //    if (moveInput != Vector2.zero)
    //    {
    //        TurnCheck(moveInput);

    //        Vector2 targetVelocity = Vector2.zero;

    //        if (InputManager.RunIsHeld)
    //        {
    //            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
    //        }

    //        else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

    //        _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
    //    }

    //    else if (moveInput == Vector2.zero)
    //    {
    //        _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
    //        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
    //    }

    //}


    // without run
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        // Prevent movement if the player cannot move or is dead
        if (!CanMove || !IsAlive)
        {
            _moveVelocity = Vector2.zero;
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
            return;
        }

        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }
        else
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }

    }

    public bool IsFacingRight()
    {
        return _isFacingRight;
    }

    private void TurnCheck(Vector2 moveInput)
    {
        bool shouldFaceRight = moveInput.x > 0;
        bool shouldFaceLeft = moveInput.x < 0;

        if (shouldFaceRight && !_isFacingRight)
        {
            Flip();
        }
        else if (shouldFaceLeft && _isFacingRight)
        {
            Flip();
        }
    }

    // animator
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.localScale *= new Vector2(-1, 1);
    }


    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Jump
    private void JumpChecks()
    {

        // Prevent jumping when the player is dead
        if (!IsAlive)
        {
            return;
        }

        // when jump is pressed
        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
            audioManager.PlaySFX(audioManager.jump); //audio sfx
        }

        // when jump is released
        if (InputManager.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        // initiate jump with jump buffering and coyote time
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        // double jump
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }

        // handle air jump after the coyote time has lapsed (take off an extra jump so the player does not get a bonus jump)
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }

        // landing
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
        animator.SetTrigger(AnimationStrings.jumpTrigger);
    }

    private void Jump()
    {
        // apply gravity while jumping
        if (_isJumping)
        {
            // check for head bump
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            // gravity on ascending 
            if (VerticalVelocity >= 0f)
            {
                // apex controls
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > MoveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;

                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
                // gravity on descending but not past apex threshold
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }

            // gravity on descending
            else if (!_isFastFalling)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        // jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        // normal gravity while falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        // clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 80f);

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
    }

    #endregion

    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

        if (_groundHit.collider != null)
        {
            _isGrounded = true;

        }
        else { _isGrounded = false; }

        #region Debug Visualizaton
        if (MoveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

        if (_headHit.collider != null)
        {
            _bumpedHead = true;
            audioManager.PlaySFX(audioManager.wallTouch); //audio sfx 

        }
        else { _bumpedHead = false; }

        #region Debug Visualizaton 
        if (MoveStats.DebugShowHeadBumpBox)
        {
            float headWidth = MoveStats.HeadWidth;

            Color rayColor;
            if (_bumpedHead)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }

        #endregion 
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }
    #endregion

    #region Timers

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
    }

    #endregion

    #region On Trigger / On Collision

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player touches a key
        if (other.CompareTag("Key"))
        {
            Key key = other.GetComponent<Key>();

            if (key != null)
            {
                Debug.Log("Player collected a key.");
                audioManager.PlaySFX(audioManager.keyPickup); //audio sfx
                Destroy(other.gameObject); // Remove the key
            }
        }


        //// Water hazard - player drowns
        //else if (other.gameObject.CompareTag("Water"))
        //{
        //    // Deal fatal damage to drown the player
        //    damageable.Hit(damageable.Health, Vector2.zero);
        //    audioManager.PlaySFX(audioManager.death); //audio sfx

        //}

        else if (other.gameObject.CompareTag("Water"))
        {
            StartCoroutine(PlayWaterSplashThenDie());
        }


    }

    #endregion

    #region Mouse
    
    public void PickupMouse(Mice newMouse)
    {
        if (currentMouse != null)
        {
            // Drop the current mouse before picking a new one
            Vector3 dropPosition = transform.position;
            currentMouse.DropMouse(dropPosition);
            currentMouse = null;
        }

        // Start pickup animation sequence
        StartCoroutine(PlayPickupSequence(newMouse));
    }

    // when player presses the pickup button
    private IEnumerator PlayPickupSequence(Mice newMouse)
    {
        // Disable movement and pickup temporarily
        animator.SetBool(AnimationStrings.canMove, false);
        InputManager.DisablePickupTemporarily = true;

        // Trigger the correct animation
        if (newMouse.isBlueMouse)
        {
            animator.SetTrigger(AnimationStrings.pickupWhiteTrigger);
        }
        else
        {
            animator.SetTrigger(AnimationStrings.pickupOrangeTrigger);
        }

        // Play audio only on manual pickup
        audioManager.PlaySFX(audioManager.micePickup);

        // Wait for the animation to finish (assuming ~0.6s; adjust as needed)
        yield return new WaitForSeconds(0.6f);

        // Actually place the mouse on the player's head
        currentMouse = newMouse;
        currentMouse.SetOnPlayerHead(transform);
        currentMouse.ShowSpriteAfterDelay(0.6f);
        currentMouse.ShowLightAfterDelay(0.6f, 0.67f);

        // Re-enable movement and pickup
        animator.SetBool(AnimationStrings.canMove, true);
        InputManager.DisablePickupTemporarily = false;

        // Toggle platform state
        ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);
    }

    //private IEnumerator PlayPickupSequence(Mice newMouse)
    //{
    //    // Disable movement and pickup temporarily
    //    animator.SetBool(AnimationStrings.canMove, false);
    //    InputManager.DisablePickupTemporarily = true;

    //    // Trigger the correct animation
    //    if (newMouse.isBlueMouse)
    //    {
    //        animator.SetTrigger(AnimationStrings.pickupWhiteTrigger);
    //    }
    //    else
    //    {
    //        animator.SetTrigger(AnimationStrings.pickupOrangeTrigger);
    //    }

    //    // Wait for the animation to finish (assuming ~0.6s; adjust as needed)
    //    yield return new WaitForSeconds(0.6f);

    //    // Actually place the mouse on the player's head
    //    currentMouse = newMouse;
    //    currentMouse.SetOnPlayerHead(transform);
    //    currentMouse.ShowSpriteAfterDelay(0.6f);
    //    currentMouse.ShowLightAfterDelay(0.6f, 0.67f);


    //    // Re-enable movement and pickup
    //    animator.SetBool(AnimationStrings.canMove, true);
    //    InputManager.DisablePickupTemporarily = false;

    //    // Toggle platform state
    //    ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);
    //}


    // when player respawns - instant spawn in without delay
    public void PickupMouseInstantly(Mice newMouse)
    {
        if (currentMouse != null)
        {
            Vector3 dropPosition = transform.position;
            currentMouse.DropMouse(dropPosition);
            currentMouse = null;
        }

        currentMouse = newMouse;
        currentMouse.SetOnPlayerHead(transform);

        // Instantly show sprite and light without delay
        SpriteRenderer sr = currentMouse.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }

        Light2D mouseLight = currentMouse.GetComponentInChildren<Light2D>();
        if (mouseLight != null)
        {
            mouseLight.intensity = 0.67f;
        }

        ToggleCorrespondingPlatforms(currentMouse.isBlueMouse);
        animator.SetBool(AnimationStrings.canMove, true);
    }

    private IEnumerator FinishPickupAfterDelay(Mice mouse)
    {
        yield return new WaitForSeconds(0.5f); // Adjust this to match your animation length

        // Re-enable movement
        animator.SetBool(AnimationStrings.canMove, true);
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Actually place the mouse on the head AFTER animation
        mouse.SetOnPlayerHead(transform);

        // Toggle platforms
        ToggleCorrespondingPlatforms(mouse.isBlueMouse);
    }


    private void ToggleCorrespondingPlatforms(bool isBlue)
    {
        PlatformToggle[] toggles = FindObjectsByType<PlatformToggle>(FindObjectsSortMode.None);

        foreach (PlatformToggle toggle in toggles)
        {
            if (toggle.isBluePlatform)
            {
                toggle.SetPlatformState(isBlue); // Activate blue platforms if picking up blue mouse
            }
            else
            {
                toggle.SetPlatformState(!isBlue); // Activate red platforms if picking up red mouse
            }
        }
    }
    public void SaveMouseState()
    {
        if (currentMouse != null)
        {
            savedHasMouse = true;
            savedMouseIsBlue = currentMouse.isBlueMouse;
            Debug.Log("[Checkpoint] Saved mouse: " + (savedMouseIsBlue ? "Blue" : "Red"));
        }
        else
        {
            savedHasMouse = false;
            Debug.Log("[Checkpoint] No mouse to save");
        }
    }

    public void RestoreSavedMouseImmediately()
    {
        if (!savedHasMouse) return;

        GameObject prefab = savedMouseIsBlue ? blueMousePrefab : redMousePrefab;
        if (prefab == null)
        {
            Debug.LogError("[Respawn] Mouse prefab is missing!");
            return;
        }

        GameObject mouseObj = Instantiate(prefab, transform.position, Quaternion.identity);
        Mice mouseScript = mouseObj.GetComponent<Mice>();

        if (mouseScript == null)
        {
            Debug.LogError("[Respawn] Instantiated object has no Mice script!");
            return;
        }

        Debug.Log("[Respawn] Restoring mouse immediately: " + (savedMouseIsBlue ? "Blue" : "Red"));
        PickupMouseInstantly(mouseScript);
    }


    #endregion

    #region Death / Hit / Knockback / Checkpoint

    public void TriggerPlayerDeath()
    {
        if (!IsAlive)
        {
            Debug.LogWarning("[Player] Tried to die but is already dead");
            return;
        }

        Debug.Log("[Player] TriggerPlayerDeath called");

        // manually mark player as dead
        IsAlive = false;
        
        // play death animation
        animator.SetBool(AnimationStrings.isAlive, false);

        // play death audio
        audioManager.PlaySFX(audioManager.death);

        PlayerDied?.Invoke();
        Debug.Log("[Player] PlayerDied event INVOKED");

        _rb.linearVelocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            enemy.StopTargetingPlayer();
        }

        if (currentMouse != null)
        {
            currentMouse.FadeAndDestroy();
            currentMouse = null;
        }
    }

    //public void RespawnPlayer()
    //{
    //    // Notify all enemies that the player is back
    //    Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    //    foreach (Enemy enemy in enemies)
    //    {
    //        enemy.ResetEnemyTargeting();
    //    }

    //    Debug.Log("[DEBUG] Player respawned, enemies can attack again.");
    //}

    public void ResetHealthAndState()
    {
        IsAlive = true;

        animator.Rebind(); // Reset animation state machine
        animator.Update(0f);
        animator.SetBool(AnimationStrings.isAlive, true);
        animator.SetBool(AnimationStrings.canMove, true);

        VerticalVelocity = 0f;
        _isJumping = false;
        _isFalling = false;
        _isFastFalling = false;
        _fastFallTime = 0f;
        _fastFallReleaseSpeed = 0f;
        _numberOfJumpsUsed = 0;
        _apexPoint = 0f;
        _timePastApexThreshold = 0f;
        _isPastApexThreshold = false;
        _jumpBufferTimer = 0f;
        _jumpReleasedDuringBuffer = false;
        _coyoteTimer = MoveStats.JumpCoyoteTime;

        _moveVelocity = Vector2.zero;
        _rb.linearVelocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        isKnockbackActive = false;
        _knockbackVelocity = Vector2.zero;

        damageable.ResetHealth();
    }

    public void ClearMouseCheckpointData()
    {
        savedHasMouse = false;
        savedMouseIsBlue = false;
    }

    //public void OnHit(int damage, Vector2 knockback)
    //{
    //    Debug.Log($"OnHit called! Applying knockback: {knockback}");

    //    StopAllCoroutines(); // Prevent stacking knockbacks
    //    StartCoroutine(ApplyKnockback(knockback));
    //    audioManager.PlaySFX(audioManager.enemyHit);
    //}

    public void OnHit(int damage, Vector2 knockback)
    {
        Debug.Log($"OnHit called! Applying knockback: {knockback}");

        StopAllCoroutines(); // Prevent stacking knockbacks
        StartCoroutine(ApplyKnockback(knockback));

        if (!Spike.wasHitBySpike)
        {
            audioManager.PlaySFX(audioManager.enemyHit);
        }
    }


    private IEnumerator ApplyKnockback(Vector2 knockback)
    {
        float knockbackDuration = 0.2f; // Adjust duration as needed
        isKnockbackActive = true;
        _knockbackVelocity = knockback;

        Debug.Log($"Knockback started: {_knockbackVelocity}");

        yield return new WaitForSeconds(knockbackDuration);

        isKnockbackActive = false;
        _knockbackVelocity = Vector2.zero;

        Debug.Log("Knockback ended");
    }

    #endregion

    #region Audio

    // for walking audio
    private void HandleFootsteps()
    {
        bool shouldPlay = _isGrounded && InputManager.Movement.x != 0 && IsAlive && CanMove;

        if (shouldPlay && !footstepsSource.isPlaying)
            footstepsSource.Play();
        else if (!shouldPlay && footstepsSource.isPlaying)
            footstepsSource.Stop();
    }

    // for water death audio
    private IEnumerator PlayWaterSplashThenDie()
    {
        // Play splash sound
        audioManager.PlaySFX(audioManager.waterSplash);

        // Delay a bit before triggering death
        yield return new WaitForSeconds(0.2f);

        // Now call Hit which will trigger TriggerPlayerDeath internally
        damageable.Hit(damageable.Health, Vector2.zero);
    }

    #endregion
}