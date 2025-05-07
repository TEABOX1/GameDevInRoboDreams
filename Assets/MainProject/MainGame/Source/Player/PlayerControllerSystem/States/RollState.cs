using System.Collections.Generic;
using GlobalSource;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class RollState : StateBase
    {
        private CharacterController _characterController;
        private InputController _inputController;
        
        private Vector3 _rollDirection ;
        private Vector3 _velocity;
        
        private float _rollSpeed;
        private float _drag;
        private float _stopThreshold = 0.1f;
        
        public RollState(
            StateMachine stateMachine,
            byte stateId,
            CharacterController characterController,
            float rollSpeed,
            float drag) : base(stateMachine, stateId)
        {
            _characterController = characterController;
            _rollSpeed = rollSpeed;
            _drag = drag;

            conditions = new List<IStateCondition>
            {
                new BaseCondition((byte)PlayerControllerState.Idle, RollComplete)
            };
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnMovementInput += MovementHandler;
        }
        
        public override void Enter()
        {
            Vector3 inputDirection = _rollDirection.normalized;
            if (inputDirection == Vector3.zero)
            {
                inputDirection = _characterController.transform.forward;
            }

            _velocity = inputDirection * _rollSpeed;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, _drag * deltaTime);
            
            _ = _characterController.Move(_velocity * deltaTime);
        }
        
        public override void Dispose()
        {
            _inputController.OnMovementInput -= MovementHandler;
        }
        
        private void MovementHandler(Vector2 input, InputDevice inputDevice)
        {
            _rollDirection  = new Vector3(input.x, 0, input.y);
        }

        private bool RollComplete()
        {
            return _velocity.magnitude <= _stopThreshold;
        }
    }
}