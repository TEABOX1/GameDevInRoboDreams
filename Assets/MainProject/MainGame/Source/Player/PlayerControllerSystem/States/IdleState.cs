using System.Collections.Generic;
using GlobalSource;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class IdleState : StateBase
    {
        private readonly CharacterController _characterController;
        private Vector3 _localDirection;
        
        private InputController _inputController;
        
        public IdleState(StateMachine stateMachine,
            byte stateId,
            CharacterController characterController) : base(stateMachine, stateId)
        {
            _characterController = characterController;
            
            conditions = new List<IStateCondition>
            {
                new BaseCondition((byte)PlayerControllerState.Movement, IsMoving),
                new BaseCondition((byte)PlayerControllerState.Fall, IsFalling),
            };
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            
            _inputController.OnMovementInput += MovementHandler;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            _ = _characterController.Move(Physics.gravity * deltaTime);
        }
        
        public override void Dispose()
        {
            _inputController.OnMovementInput -= MovementHandler;
        }
        
        private void MovementHandler(Vector2 input, InputDevice inputDevice)
        {
            _localDirection = new Vector3(input.x, 0, input.y);
        }
        
        private bool IsMoving()
        {
            return !Mathf.Approximately(_localDirection.sqrMagnitude, 0f);
        }
        
        private bool IsFalling()
        {
            return !_characterController.isGrounded;
        }
    }
}