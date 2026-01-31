using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 Movement;

    public static bool DefaultAttackWasPressed;
    public static bool DefaultAttackIsHeld;
    public static bool SpecialAttackWasPressed;
    public static bool SkillWasPressed;

    public static bool CameraMoveIsheld;
    public static bool CameraMoveIsReleased;

    private InputAction _moveAction;
    private InputAction _defaultAttackAction;
    private InputAction _specialAttackAction;
    private InputAction _interactAction;
    private InputAction _cameraMoveAction;


    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _defaultAttackAction = PlayerInput.actions["DefaultAttack"];
        _specialAttackAction = PlayerInput.actions["SpecialAttack"];
        _interactAction = PlayerInput.actions["Interact"];
        _cameraMoveAction = PlayerInput.actions["CameraMove"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        DefaultAttackWasPressed = _defaultAttackAction.WasPressedThisFrame();

        DefaultAttackIsHeld = _defaultAttackAction.IsPressed();

        SpecialAttackWasPressed = _specialAttackAction.WasPressedThisFrame();

        SkillWasPressed = _interactAction.WasPressedThisFrame();

        CameraMoveIsheld = _cameraMoveAction.IsPressed();

        CameraMoveIsReleased = _cameraMoveAction.WasPressedThisFrame();


    }
}