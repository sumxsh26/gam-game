using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool RunIsHeld;
    public static bool RestartWasPressed;
    public static bool PauseWasPressed;
    public static bool PickupWasPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _restartAction;
    private InputAction _pauseAction;
    private InputAction _pickupAction;

    private float pickupBufferTime = 0.2f; // Time in seconds to remember input
    private float pickupBufferTimer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        _restartAction = PlayerInput.actions["Restart"];
        _pauseAction = PlayerInput.actions["Pause"];
        _pickupAction = PlayerInput.actions["Pickup"];
    }

    // Update is called once per frame
    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        RunIsHeld = _runAction.IsPressed();

        RestartWasPressed = _restartAction.WasPressedThisFrame();

        PauseWasPressed = _pauseAction.WasPressedThisFrame();

        PickupWasPressed = _pickupAction.WasPressedThisFrame();

        if (_pickupAction.WasPressedThisFrame())
        {
            PickupWasPressed = true;
            pickupBufferTimer = pickupBufferTime; // Start buffer countdown
        }

        if (pickupBufferTimer > 0)
        {
            pickupBufferTimer -= Time.deltaTime;
        }
        else
        {
            PickupWasPressed = false; // Reset after buffer time expires
        }

    }
}
