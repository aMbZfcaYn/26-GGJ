using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager: MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 Movement;

    public static bool DefaultAttackWasPressed;
    public static bool SpecialAttackWasPressed;
    public static bool SkillWasPressed;

    private InputAction _moveAction;
    private InputAction _defaultAttackAction;
    private InputAction _specialAttackAction;
    private InputAction _interactAction;

    private void Awake ()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _defaultAttackAction = PlayerInput.actions["DefaultAttack"];
        _specialAttackAction = PlayerInput.actions["SpecialAttack"];
        _interactAction = PlayerInput.actions["Interact"];
    }

    private void Update ()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        DefaultAttackWasPressed = _defaultAttackAction.WasPressedThisFrame();
        
        SpecialAttackWasPressed = _specialAttackAction.WasPressedThisFrame();
        
        SkillWasPressed = _interactAction.WasPressedThisFrame();

    }
}