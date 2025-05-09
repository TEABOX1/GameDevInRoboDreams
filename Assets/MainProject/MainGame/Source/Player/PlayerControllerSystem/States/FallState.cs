using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class FallState : StateBase
    {
        private readonly CharacterController _characterController;
        
        private Vector3 _jumpDirection;
        private Vector3 _fallDirection;
        
        
        private Vector3 _velocity;
        private float _drag;
        
        public FallState(
            StateMachine stateMachine,
            byte stateId,
            CharacterController characterController,
            float drag) : base(stateMachine, stateId)
        {
            _characterController = characterController;
            _drag = drag;
            
            conditions = new List<IStateCondition>
            {
                new BaseCondition((byte)PlayerControllerState.Idle, Landed)
            };
        }

        public override void Enter()
        {
            _velocity = _characterController.velocity;
            _velocity.y = 0f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            float verticalVelocity = _velocity.y;
            _velocity.y = 0f;
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, _drag * deltaTime);
            _velocity.y = verticalVelocity;
            
            _velocity += Physics.gravity * deltaTime;
            
            _ = _characterController.Move(_velocity * deltaTime);
        }
        
        public override void Dispose()
        {
        }

        private bool Landed()
        {
            return _characterController.isGrounded;
        }
    }
}