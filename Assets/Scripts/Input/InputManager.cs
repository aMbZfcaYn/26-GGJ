using UnityEngine;
using UnityEngine.InputSystem;

namespace InputNamespace
{
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
        
        private static bool _isGameInputActive = false;

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
            if (!_isGameInputActive)
            {
                ResetInputs();
                return; // 直接返回，不再读取硬件输入
            }
            
            Movement = _moveAction.ReadValue<Vector2>();

            DefaultAttackWasPressed = _defaultAttackAction.WasPressedThisFrame();

            DefaultAttackIsHeld = _defaultAttackAction.IsPressed();

            SpecialAttackWasPressed = _specialAttackAction.WasPressedThisFrame();

            SkillWasPressed = _interactAction.WasPressedThisFrame();

            CameraMoveIsheld = _cameraMoveAction.IsPressed();

            CameraMoveIsReleased = _cameraMoveAction.WasPressedThisFrame();
        }
        
        private void ResetInputs()
        {
            Movement = Vector2.zero;
            DefaultAttackWasPressed = false;
            DefaultAttackIsHeld = false;
            SpecialAttackWasPressed = false;
            SkillWasPressed = false;
            CameraMoveIsheld = false;
            CameraMoveIsReleased = false;
        }

        // --- 新增：外部调用的开关 ---
        /// <summary>
        /// 切换输入模式
        /// </summary>
        /// <param name="enableGameInput">true: 游戏控制(隐藏鼠标); false: UI控制(显示鼠标)</param>
        public static void SetGameInputState(bool enableGameInput)
        {
            _isGameInputActive = enableGameInput;

            if (enableGameInput)
            {
                // 进入游戏状态：锁定鼠标，隐藏光标
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
            }
            else
            {
                // 进入UI状态：解锁鼠标，显示光标
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
                
                // 也可以在这里切换 ActionMap，如果你有专门的UI Map
                // PlayerInput.SwitchCurrentActionMap("UI");
            }
        }
    }
}