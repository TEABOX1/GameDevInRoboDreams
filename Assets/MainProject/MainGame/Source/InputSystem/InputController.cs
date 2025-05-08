using System;
using GlobalSource;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class InputController : MonoServiceBase
    {
        public override Type Type { get; } = typeof(InputController);
        
        public event Action<Vector2, InputDevice> OnMovementInput;
        public event Action<Vector2> OnLookAroundInput;
        public event Action OnJumpInput;
        public event Action OnRollInput;
        public event Action OnPrimaryInput;
        public event Action<bool> OnSecondaryInput;
        public event Action OnInteractInput;
        
        public event Action OnEscapeInput;
        
        [Header("Input Action Asset")]
        [SerializeField] private InputActionAsset _inputActionAsset;
        
        [Header("Input Action Maps")]
        [SerializeField, ActionMapDropdown] private string _defaultMapName;
        [SerializeField, ActionMapDropdown] private string _uiMapName;

        [Header("Input Actions")]
        [SerializeField, ActionInputDropdown] private string _movementInputName;
        [SerializeField, ActionInputDropdown] private string _lookAroundInputName;
        [SerializeField, ActionInputDropdown] private string _jumpInputName;
        [SerializeField, ActionInputDropdown] private string _rollInputName;
        [SerializeField, ActionInputDropdown] private string _primaryInputName;
        [SerializeField, ActionInputDropdown] private string _secondaryInputName;
        [SerializeField, ActionInputDropdown] private string _interactInputName;
        
        [SerializeField, ActionInputDropdown] private string _escapeInputName;
        
        private InputAction _movementAction;
        private InputAction _lookAroundAction;
        private InputAction _jumpAction;
        private InputAction _rollAction;
        private InputAction _primaryAction;
        private InputAction _secondaryAction;
        private InputAction _interactAction;
        
        private InputAction _escapeAction;
        
        private InputActionMap _defaultActionMap;
        private InputActionMap _uiActionMap;

        protected override void Awake()
        {
            base.Awake();
            
            _inputActionAsset.Enable();
            
            _defaultActionMap = _inputActionAsset?.FindActionMap(_defaultMapName);
            _uiActionMap = _inputActionAsset?.FindActionMap(_uiMapName);
            
            _movementAction = _defaultActionMap?.FindAction(_movementInputName);
            _lookAroundAction = _defaultActionMap?.FindAction(_lookAroundInputName);
            _jumpAction = _defaultActionMap?.FindAction(_jumpInputName);
            _rollAction = _defaultActionMap?.FindAction(_rollInputName);
            _primaryAction = _defaultActionMap?.FindAction(_primaryInputName);
            _secondaryAction = _defaultActionMap?.FindAction(_secondaryInputName);
            _interactAction = _defaultActionMap?.FindAction(_interactInputName);
            
            _escapeAction = _uiActionMap?.FindAction(_escapeInputName);

            if (_inputActionAsset)
            {
                _movementAction.performed += MovementPerformedHandler;
                _movementAction.canceled += MovementCanceledHandler;
                
                _lookAroundAction.performed += LookAroundPerformedHandler;
                
                _jumpAction.performed += JumpPerformedHandler;
                
                _rollAction.performed += RollPerformedHandler;
                
                _primaryAction.performed += PrimaryPerformedHandler;
                
                _secondaryAction.performed += SecondaryPerformedHandler;
                _secondaryAction.canceled += SecondaryCanceledHandler;
                
                _interactAction.performed += InteractPerformedHandler;
                
                _escapeAction.performed += EscapePerformedHandler;
            }
            else
            {
                Debug.LogError("Input action asset is missing.");
            }
        }
        
        private void OnDisable()
        {
            // CursorEnable();
            // _defaultActionMap.Disable();
            // _uiActionMap.Disable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _movementAction.performed -= MovementPerformedHandler;
            _movementAction.canceled -= MovementCanceledHandler;
            
            _lookAroundAction.performed -= LookAroundPerformedHandler;
            
            _jumpAction.performed -= JumpPerformedHandler;
            
            _rollAction.performed -= RollPerformedHandler;
            
            _primaryAction.performed -= PrimaryPerformedHandler;
            
            _secondaryAction.performed -= SecondaryPerformedHandler;
            _secondaryAction.canceled -= SecondaryCanceledHandler;
            
            _interactAction.performed -= InteractPerformedHandler;
            
            _escapeAction.performed -= EscapePerformedHandler;
            
            OnMovementInput = null;
            OnLookAroundInput = null;
            OnJumpInput = null;
            OnRollInput = null;
            OnPrimaryInput = null;
            OnSecondaryInput = null;
            OnInteractInput = null;
            OnEscapeInput = null;
        }
        
        public void CursorEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void CursorDisable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void DefaulMapLock()
        {
            _defaultActionMap.Disable();
        }

        public void UIMapLock()
        {
            _uiActionMap.Disable();
            CursorEnable();
        }

        public void DefaultMapUnlock()
        {
            _defaultActionMap.Enable();
        }

        public void UIMapUnlock()
        {
            _uiActionMap.Enable();
            CursorDisable();
        }
        
        private void MovementPerformedHandler(InputAction.CallbackContext context)
        {
            OnMovementInput?.Invoke(context.ReadValue<Vector2>(), context.control.device);
        }
        private void MovementCanceledHandler(InputAction.CallbackContext context)
        {
            OnMovementInput?.Invoke(context.ReadValue<Vector2>(), context.control.device);
        }
        
        private void LookAroundPerformedHandler(InputAction.CallbackContext context)
        {
            OnLookAroundInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void JumpPerformedHandler(InputAction.CallbackContext context)
        {
            OnJumpInput?.Invoke();
        }

        private void RollPerformedHandler(InputAction.CallbackContext context)
        {
            OnRollInput?.Invoke();
        }

        private void PrimaryPerformedHandler(InputAction.CallbackContext context)
        {
            OnPrimaryInput?.Invoke();
        }

        private void SecondaryPerformedHandler(InputAction.CallbackContext context)
        {
            OnSecondaryInput?.Invoke(true);
        }
        private void SecondaryCanceledHandler(InputAction.CallbackContext context)
        {
            OnSecondaryInput?.Invoke(false);
        }

        private void InteractPerformedHandler(InputAction.CallbackContext context)
        {
            OnInteractInput?.Invoke();
        }

        private void EscapePerformedHandler(InputAction.CallbackContext context)
        {
            OnEscapeInput?.Invoke();
        }
    }
}
