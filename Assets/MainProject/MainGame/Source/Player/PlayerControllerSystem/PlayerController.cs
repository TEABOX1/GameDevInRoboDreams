using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PlayerControllerState> OnStateChanged;
        
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Health _health;

        [Header("PlayerSettings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _drag;
        [SerializeField] private Vector2 _jumpSpeed;
        [SerializeField] private float _rollSpeed;
        [SerializeField] private AnimationCurve _rollCurve;
        
        private StateMachine _stateMachine;
        private ISaveService _saveService;
        
        private Transform _playerTransform;

        public StateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public float Speed => _speed;
        // public string CurrentState => _stateMachine == null ? "[NULL]" : _stateMachine.CurrentState.GetType().Name;
        public PlayerControllerState PlayerControllerState => (PlayerControllerState)_stateMachine.CurrentState.StateId;

        private void Awake()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            _playerTransform = transform;
        }

        private void OnEnable()
        {
            LoadPlayerInfo();
        }

        private void OnDisable()
        {
            SavePlayerInfo();
        }
        
        private void Start()
        {
            ServiceLocator.Instance.GetService<IHealthService>().AddCharacter(_health);
                
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
                    _rollSpeed, /*_drag,*/ _rollCurve));
            
            _stateMachine.InitState((byte)PlayerControllerState.Idle);
            
            _stateMachine.OnStateChange += StateChangeHandler;
        }
        
        private void FixedUpdate()
        {
            _stateMachine.Update(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            SavePlayerInfo();
            
            _saveService.SaveAll();
            
            _stateMachine?.Dispose();
        }

        private void SavePlayerInfo()
        {
            _saveService.SaveData.playerInfoData.PlayerPosition = _playerTransform.localPosition;
            // _saveService.SaveData.playerInfoData.PlayerRotationY = _playerTransform.eulerAngles.y;
            _saveService.SaveData.playerInfoData.HealthValue = _health.HealthValue;
        }

        private void LoadPlayerInfo()
        {
            _health.SetHealth(_saveService.SaveData.playerInfoData.HealthValue);
            _playerTransform.position = _saveService.SaveData.playerInfoData.PlayerPosition;
            // _playerTransform.rotation = Quaternion.Euler(0f, _saveService.SaveData.playerInfoData.PlayerRotationY, 0f);
        }
        
        private void StateChangeHandler(byte stateId)
        {
            OnStateChanged?.Invoke((PlayerControllerState)stateId);
        }
    }
}