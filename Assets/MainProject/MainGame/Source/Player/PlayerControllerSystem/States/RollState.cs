using System.Collections.Generic;
using GlobalSource;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class RollState : StateBase
    {
        private readonly CharacterController _characterController;
        
        private Vector3 _velocity;
        
        private readonly float _rollSpeed;
        // private readonly float _drag;
        private readonly AnimationCurve _rollCurve;
        
        private Vector3 _startVelocity;
        private float _elapsedTime;
        private float _rollDuration;
        // private float _stopThreshold = 0.1f;
        
        private Vector2 _movementInput;
        
        private InputController _inputController;
        
        public RollState(
            StateMachine stateMachine,
            byte stateId,
            CharacterController characterController,
            float rollSpeed,
            /*float drag,*/
            AnimationCurve rollCurve) : base(stateMachine, stateId)
        {
            _characterController = characterController;
            _rollSpeed = rollSpeed;
            // _drag = drag;
            _rollCurve = rollCurve;

            conditions = new List<IStateCondition>
            {
                new BaseCondition((byte)PlayerControllerState.Idle, RollComplete)
            };
            
            if (_rollCurve.length > 0)
            {
                _rollDuration = _rollCurve[_rollCurve.length - 1].time;
            }
            else
            {
                _rollDuration = 1f;
            }
        }
        
        public override void Enter()
        {
            _elapsedTime = 0f;
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _movementInput = _inputController.GetMovementInput();
            
            // Vector3 direction = _characterController.velocity.normalized;
            Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y);
            direction = Quaternion.Euler(0, _characterController.transform.eulerAngles.y, 0) * direction;
            direction.Normalize();
            
            if (direction == Vector3.zero)
            {
                direction = _characterController.transform.forward;
            }
            
            Vector3 currentVelocity = _characterController.velocity;
            float forwardSpeed = Vector3.Dot(currentVelocity, direction);
            float dynamicRollSpeed = _rollSpeed + forwardSpeed * 0.5f;
            // _startVelocity = direction * _rollSpeed;
            _startVelocity = direction * dynamicRollSpeed;
            // _velocity = direction * _rollSpeed;
            
            _inputController.DefaulMapLock();
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            _elapsedTime += deltaTime;
            float curveValue = _rollCurve.Evaluate(_elapsedTime);
            _velocity = _startVelocity * curveValue;
            // _velocity = Vector3.Lerp(_velocity, Vector3.zero, _drag * deltaTime);
            
            _ = _characterController.Move(_velocity * deltaTime);
        }
        
        public override void Dispose()
        {
        }

        public override void Exit()
        {
            _inputController.DefaultMapUnlock();
        }
        
        private void MovementInputHandler(Vector2 movementInput, InputDevice device)
        {
            _movementInput = movementInput;
        }
        
        private bool RollComplete()
        {
            // return _velocity.magnitude <= _stopThreshold;
            return _elapsedTime >= _rollDuration;
        }
    }
}