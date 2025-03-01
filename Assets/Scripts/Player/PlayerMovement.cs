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
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public Key cm;
    public event Action PlayerDied;


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

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

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

            Vector2 targetVelocity = Vector2.zero;

            if (InputManager.RunIsHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
            }

            else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }

        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }

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
                Destroy(other.gameObject); // Remove the key
            }
        }


        // Water hazard - player drowns
        else if (other.gameObject.CompareTag("Water"))
        {
            // Deal fatal damage to drown the player
            damageable.Hit(damageable.Health, Vector2.zero);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Mice"))
        {
            Mice mouse = other.GetComponent<Mice>();

            if (mouse != null)
            {
                // Destroy the mouse GameObject immediately upon pickup
                Destroy(mouse.gameObject);

                // Toggle all platforms upon pickup
                TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
                foreach (TilemapToggle toggle in toggles)
                {
                    toggle.TogglePlatform();
                }

                Debug.Log("Mouse picked up and platforms toggled!");
            }
        }
    }


    #endregion

    #region Toggle Platforms

    private void ToggleAllPlatforms()
    {
        TilemapToggle[] toggles = FindObjectsByType<TilemapToggle>(FindObjectsSortMode.None);
        foreach (TilemapToggle toggle in toggles)
        {
            toggle.TogglePlatform();
        }
    }
    #endregion

    #region Death / Hit / Knockback

    public void TriggerPlayerDeath()
    {
        PlayerDied?.Invoke();
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        Debug.Log($"OnHit called! Applying knockback: {knockback}");

        StopAllCoroutines(); // Prevent stacking knockbacks
        StartCoroutine(ApplyKnockback(knockback));
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
}