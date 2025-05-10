using GlobalSource;
using System;
using UnityEngine;

namespace MainGame
{
    public class PlayerRadar : MonoBehaviour, IPlayerRadar
    {
        public enum PlayerdarState
        {
            Scanning = 0,
            Chasing = 1,
            Searching = 2,

            NullState = 255
        }

        public event Action<PlayerdarState> OnPlayerdarStateChange;
        public event Action OnLookAround;

        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private float _range;
        [SerializeField] private float _angle;
        //[SerializeField] private LayerMask _layerMask;

        private float _cosine;

        private Transform _transform;
        private Transform _currentTarget;

        private IPlayerService _playerService;
        private bool _hasTarget;
        private bool _seesTarget;
        private Vector3 _lastTargetPosition;

        private PlayerdarState _currentState;
        public PlayerdarState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;
                _currentState = value;
                _enemyController.ComputeBehaviour();
            }
        }

        public Transform CurrentTarget => _currentTarget;
        public IPlayerService PlayerService => _playerService;
        public bool HasTarget => _hasTarget;
        public bool SeesTarget => _seesTarget;
        public Vector3 LastTargetPosition => _lastTargetPosition;

        private void Start()
        {
            _transform = transform;
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
            _cosine = Mathf.Cos(_angle * Mathf.Deg2Rad);

            _enemyController.Health.OnHealthChanged += HealthChangedHandler;
        }

        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case PlayerdarState.Scanning:
                    ScanningUpdate();
                    break;
                case PlayerdarState.Chasing:
                    ChasingUpdate();
                    break;
                case PlayerdarState.Searching:
                    SearchingUpdate();
                    break;
            }
        }

        private void ScanningUpdate()
        {
            if (!CheckTarget(_playerService.Player.TargetPivot))
                return;

            _currentTarget = _playerService.Player.TargetPivot;
            _hasTarget = true;
            _seesTarget = true;
            ChangeState((byte)PlayerdarState.Chasing);
            //CurrentState = PlayerdarState.Chasing;
        }

        private void ChasingUpdate()
        {
            _lastTargetPosition = _currentTarget.position;

            if (CheckTarget(_currentTarget))
                return;

            _seesTarget = false;
            ChangeState((byte)PlayerdarState.Searching);
            //CurrentState = PlayerdarState.Searching;
        }

        private void SearchingUpdate()
        {
            if (!CheckTarget(_currentTarget))
                return;

            _seesTarget = true;
            ChangeState((byte)PlayerdarState.Chasing);
            //CurrentState = PlayerdarState.Chasing;
        }

        public void LookAround()
        {
            OnLookAround?.Invoke();

            if (_hasTarget)
            {
                if (CheckTarget(_currentTarget, false))
                {
                    _seesTarget = true;
                    ChangeState((byte)PlayerdarState.Chasing);
                    //CurrentState = PlayerdarState.Chasing;
                }
                else
                {
                    _seesTarget = false;
                    _hasTarget = false;
                    //ChangeState((byte)PlayerdarState.Scanning);
                    //CurrentState = PlayerdarState.Scanning;
                }
            }
            else
            {
                if (CheckTarget(_playerService.Player.TargetPivot, false))
                {
                    _hasTarget = true;
                    _seesTarget = true;
                    _currentTarget = _playerService.Player.TargetPivot;
                    ChangeState((byte)PlayerdarState.Chasing);
                    //CurrentState = PlayerdarState.Chasing;
                }
                else
                {
                    _seesTarget = false;
                    ChangeState((byte)PlayerdarState.Scanning);
                    //CurrentState = PlayerdarState.Scanning;
                }
            }
        }

        private bool CheckTarget(Transform targetable, bool useFov = true)
        {
            Vector3 position = _transform.position;
            Vector3 playerPosition = targetable.position;

            Vector3 playerDirection = Vector3.ProjectOnPlane(playerPosition - position, Vector3.up);

            if (playerDirection.sqrMagnitude > _range * _range)
                return false;

            playerDirection.Normalize();
            Vector3 forward = Vector3.ProjectOnPlane(_transform.forward, Vector3.up).normalized;

            if (useFov)
            {
                if (Vector3.Dot(playerDirection, forward) < _cosine)
                    return false;
            }
            if (!Physics.Raycast(position, (playerPosition - position).normalized, out RaycastHit hit, _range/*, _layerMask*/))
                return false;
            if (hit.collider != _playerService.Player.CharacterController)
                return false;
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (_transform == null)
                _transform = transform;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_transform.position, _range);

            Vector3 forward = _transform.forward;
            Vector3 leftBoundary = Quaternion.Euler(0, -_angle / 2f, 0) * forward;
            Vector3 rightBoundary = Quaternion.Euler(0, _angle / 2f, 0) * forward;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_transform.position, _transform.position + leftBoundary * _range);
            Gizmos.DrawLine(_transform.position, _transform.position + rightBoundary * _range);
        }

        protected void ChangeState(byte stateId)
        {
            CurrentState = (PlayerdarState)stateId;
            OnPlayerdarStateChange?.Invoke(CurrentState);
        }

        private void HealthChangedHandler(int health)
        {
            LookAround();
        }
    }
}
