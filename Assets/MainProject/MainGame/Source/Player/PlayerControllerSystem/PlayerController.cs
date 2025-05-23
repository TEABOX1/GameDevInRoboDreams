using System;
using Boot;
using GlobalSource;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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
        
        [SerializeField] private BossFightArea _bossFightArea;

        [SerializeField] private InputAction _action;

        [SerializeField] private float _healCooldown = 10f;
        [SerializeField] private int _healAmount = 5;

        private StateMachine _stateMachine;
        private ISaveService _saveService;
        private CheckpointService _checkpointService;
        
        private Transform _playerTransform;

        private bool _isInBossFight;
        
        public StateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public float Speed => _speed;
        // public string CurrentState => _stateMachine == null ? "[NULL]" : _stateMachine.CurrentState.GetType().Name;
        public PlayerControllerState PlayerControllerState => (PlayerControllerState)_stateMachine.CurrentState.StateId;

        private float _lastHealTime;

        private void Awake()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            _playerTransform = transform;
        }

        private void OnEnable()
        {
            _checkpointService = ServiceLocator.Instance.GetService<CheckpointService>();
            _checkpointService.OnCheckpointReached += CheckpointHandler;
            LoadPlayerInfo();
            ServiceLocator.Instance.GetService<GameplayPauseMenu>().OnSaveSignal += SavePlayerInfo;
            ServiceLocator.Instance.GetService<GameplayPauseMenu>().OnLoadSignal += LoadPlayerInfo;

            _action.Enable();
            _action.performed += Heal;
        }

        private void Heal(InputAction.CallbackContext context)
        {
            //_health.SetHealth(_health.MaxHealthValue);
        }

        private void OnDisable()
        {
            _checkpointService.OnCheckpointReached -= CheckpointHandler;
            // SavePlayerInfo();
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
            
            _bossFightArea.OnBossSpawn += BossSpawnHandler;
            _bossFightArea.OnBossDeath += BossDeathHandler;
        }
        
        private void FixedUpdate()
        {
            _stateMachine.Update(Time.fixedDeltaTime);
            if (Time.time >= _lastHealTime + _healCooldown)
            {
                _health.Heal(_healAmount);
                _lastHealTime = Time.time;
            }
        }

        private void OnDestroy()
        {
            // SavePlayerInfo();
            //
            // _saveService.SaveAll();
            
            _stateMachine?.Dispose();
        }

        private void SavePlayerInfo()
        {
            //TODO: Change _isInBossFight for any other check later, save just for now
            if(!_health.IsAlive || _isInBossFight) return;
            
            Debug.Log("Saving player info from PlayerController");
            
            _saveService.SaveData.playerInfoData.PlayerPosition = _playerTransform.position;
            _saveService.SaveData.playerInfoData.PlayerRotationY = _playerTransform.eulerAngles.y;
            _saveService.SaveData.playerInfoData.HealthValue = _health.HealthValue;
        }

        private void LoadPlayerInfo()
        {
            _health.SetHealth(_saveService.SaveData.playerInfoData.HealthValue);
            _playerTransform.position = _saveService.SaveData.playerInfoData.PlayerPosition;
            _playerTransform.rotation = Quaternion.Euler(0f, _saveService.SaveData.playerInfoData.PlayerRotationY, 0f);
        }
        
        private void StateChangeHandler(byte stateId)
        {
            OnStateChanged?.Invoke((PlayerControllerState)stateId);
        }

        private void CheckpointHandler()
        {
            SavePlayerInfo();
        }
        
        private void BossSpawnHandler(IHealth bossHealth)
        {
            _isInBossFight = true;
        }
        private void BossDeathHandler()
        {
            _isInBossFight = false;
        }
    }
}