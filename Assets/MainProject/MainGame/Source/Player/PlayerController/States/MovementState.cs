using System.Collections.Generic;
using GlobalSource;
using MainProject.MainGame.Source.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainProject.MainGame.Source.Player.PlayerController.States
{
    public class MovementState : StateBase
    {
        private readonly CharacterController _characterController;
        private readonly Transform _transform;
        private PlayerController _playerController;
        
        private bool _grounded;

        private Vector3 _localDirection;
        
        private InputController _inputController;
        
        public MovementState(
            StateMachine stateMachine,
            byte stateId,
            CharacterController characterController,
            Transform transform,
            PlayerController playerController) : base(stateMachine, stateId)
        {
            _characterController = characterController;
            _transform = transform;
            _playerController = playerController;

            conditions = new List<IStateCondition>
            {
                new BaseCondition((byte)PlayerControllerState.Idle, IsIdle),
            };
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            
            _inputController.OnMovementInput += MovementHandler;
        }
        
        private void MovementHandler(Vector2 input, InputDevice inputDevice)
        {
            _localDirection = new Vector3(input.x, 0, input.y);
        }

        protected override void OnUpdate(float deltaTime)
        {
            Vector3 forward = _transform.forward;
            Vector3 right = _transform.right;
            
            Vector3 direction = forward * _localDirection.z + right * _localDirection.x;
            
            _ = _characterController.Move((direction * _playerController.Speed + Physics.gravity) * deltaTime);
        }
        
        public override void Dispose()
        {
            _inputController.OnMovementInput -= MovementHandler;
        }

        private bool IsIdle()
        {
            return Mathf.Approximately(_localDirection.sqrMagnitude, 0f);
        }
        
        private bool IsFalling()
        {
            return !_characterController.isGrounded;
        }
    }
}