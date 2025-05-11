using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class RollState : StateBase
    {
        private readonly CharacterController _characterController;
        
        private Vector3 _velocity;
        
        private readonly float _rollSpeed;
        private readonly float _drag;
        private float _stopThreshold = 0.1f;
        
        private InputController _inputController;
        
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
        }
        
        public override void Enter()
        {
            //TODO: Add cooldown or lock default input action map after testing with animations
            Vector3 direction = _characterController.velocity.normalized;
            
            if (direction == Vector3.zero)
            {
                direction = _characterController.transform.forward;
            }

            _velocity = direction * _rollSpeed;
            
            // _inputController.DefaulMapLock();
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, _drag * deltaTime);
            
            _ = _characterController.Move(_velocity * deltaTime);
        }
        
        public override void Dispose()
        {
        }

        public override void Exit()
        {
            // _inputController.DefaultMapUnlock();
        }
        
        private bool RollComplete()
        {
            return _velocity.magnitude <= _stopThreshold;
        }
    }
}