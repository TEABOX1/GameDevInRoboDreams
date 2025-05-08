using System;
using UnityEngine;
using static MainGame.AttackBehaviour;

namespace MainGame
{
    public class Playerdar : MonoBehaviour, IPlayerdar
    {
        public enum State
        {
            Scanning = 0,
            Chasing = 1,
            Searching = 2,

            NullState = 255
        }

        public event Action<State> OnPlayerdarStateChange;

        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private float _range;
        [SerializeField] private float _angle;
        //[SerializeField] private LayerMask _layerMask;

        private float _cosine;

        private Transform _transform;
        //private IPlayerService _playerService; // Потрібно створити інтерфейс IPlayerService

        //private TargetableBase _currentTarget; // Потрібно створити скрипт TargetableBase
        private bool _hasTarget;
        private bool _seesTarget;
        private Vector3 _lastTargetPosition;

        private State _currentState;
        public State CurrentState
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

        //public TargetableBase CurrentTarget => _currentTarget;
        public bool HasTarget => _hasTarget;
        public bool SeesTarget => _seesTarget;
        public Vector3 LastTargetPosition => _lastTargetPosition;

        private void Start()
        {
            _transform = transform;
            //_playerService = ServiceLocator.Instance.GetService<IPlayerService>();
            _cosine = Mathf.Cos(_angle * Mathf.Deg2Rad);

            _enemyController.Health.OnHealthChanged += HealthChangedHandler;
        }

        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case State.Scanning:
                    ScanningUpdate();
                    break;
                case State.Chasing:
                    ChasingUpdate();
                    break;
                case State.Searching:
                    SearchingUpdate();
                    break;
            }
        }

        private void ScanningUpdate()
        {

        }

        private void ChasingUpdate()
        {

        }

        private void SearchingUpdate()
        {

        }

        public void LookAround()
        {
            if (_hasTarget)
            {

            }
            else
            {

            }
        }

        private void HealthChangedHandler(int health)
        {
            LookAround();
        }
    }
}
