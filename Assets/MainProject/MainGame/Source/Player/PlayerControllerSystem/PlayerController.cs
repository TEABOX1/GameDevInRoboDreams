using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PlayerControllerState> OnStateChanged;
        
        [SerializeField] private CharacterController _characterController;
        
        [Header("PlayerSettings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _drag;
        [SerializeField] private Vector2 _jumpSpeed;
        [SerializeField] private float _rollSpeed;

        private StateMachine _stateMachine;

        public StateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public float Speed => _speed;
        // public string CurrentState => _stateMachine == null ? "[NULL]" : _stateMachine.CurrentState.GetType().Name;
        // public PlayerControllerState PlayerControllerState => (PlayerControllerState)_stateMachine.CurrentState.StateId;
        
        private void Start()
        {
            _stateMachine = new StateMachine();
            
            _stateMachine.AddState((byte)PlayerControllerState.Idle,
                new IdleState(_stateMachine, (byte)PlayerControllerState.Idle, _characterController));
            
            _stateMachine.AddState((byte)PlayerControllerState.Movement,
                new MovementState(_stateMachine, (byte)PlayerControllerState.Movement, _characterController,
                    _characterController.transform, this));
            
            _stateMachine.AddState((byte)PlayerControllerState.Fall,
                new FallState(_stateMachine, (byte)PlayerControllerState.Fall, _characterController, _drag));
            
            _stateMachine.AddState((byte)PlayerControllerState.Jump,
                new JumpState(_stateMachine, (byte)PlayerControllerState.Jump, _characterController,
                    _drag, _jumpSpeed.y, _jumpSpeed.x));
            
            _stateMachine.AddState((byte)PlayerControllerState.Roll,
                new RollState(_stateMachine, (byte)PlayerControllerState.Roll, _characterController,
                    _rollSpeed, _drag));
            
            _stateMachine.InitState((byte)PlayerControllerState.Idle);
            
            _stateMachine.OnStateChange += StateChangeHandler;
        }
        
        private void FixedUpdate()
        {
            _stateMachine.Update(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            _stateMachine?.Dispose();
        }
        
        private void StateChangeHandler(byte stateId)
        {
            OnStateChanged?.Invoke((PlayerControllerState)stateId);
        }
    }
}