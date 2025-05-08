using GlobalSource;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        public enum EnemyBehaviour
        {
            Deciding = 0,
            Idle = 1,
            Patrol = 2,
            Attack = 3,
            Search = 4,
            Death = 5,

            NullState = 255
        }

        public event Action<EnemyBehaviour> OnBehaviourChanged;

        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Health _health;
        [SerializeField] private Playerdar _playerdar;

        [Header("Enemy Data")]
        [SerializeField] private Vector2 _idleDuration;
        [SerializeField] private float _maxPatrolStamina;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _chaseSpeed;
        [SerializeField] private float _lookAroundDistance;


        private INavPointProvider _navPointProvider;

        protected BehaviourTree _behaviourTree;
        protected StateMachine _behaviourMachine;

        private EnemyBehaviour _currentBehaviour;
        private float _patrolStamina;

        public float PatrolStamina
        {
            get => _patrolStamina;
            set { _patrolStamina = Mathf.Clamp(value, 0, _maxPatrolStamina); }
        }

        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public CharacterController CharacterController => _characterController;
        public Transform CharacterTransform => _characterTransform;
        public IHealth Health => _health;
        public IPlayerdar Playerdar => _playerdar;
        public INavPointProvider NavPointProvider => _navPointProvider;

        public Vector2 IdleDuration => _idleDuration;
        public float PatrolSpeed => _patrolSpeed;
        public float ChaseSpeed => _chaseSpeed;
        public float LookAroundDistance => _lookAroundDistance;

        private void Awake()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.avoidancePriority = Random.Range(0, 100);

            InitStateMachine();
            _behaviourMachine.OnStateChange += StateChangeHandler;

            InitBehaviourTree();

            _health.OnDeath += HealthDeathHandler;
        }

        protected virtual void InitStateMachine()
        {
            _behaviourMachine = new StateMachine();

            _behaviourMachine.AddState((byte)EnemyBehaviour.Deciding,
                new DecisionBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Deciding, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Idle,
                new IdleBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Idle, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Patrol,
                new PatrolBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Patrol, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Search,
                new SearchBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Search, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
                new AttackBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Death,
                new DeathBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Death, this));
        }

        protected virtual void InitBehaviourTree()
        {
            BehaviourLeaf idleLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Idle);
            BehaviourLeaf patrolLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Patrol);

            BehaviourBranch patrolBranch = new BehaviourBranch(patrolLeaf, idleLeaf, PatrolStaminaCondition);

            BehaviourLeaf attackLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Attack);
            BehaviourLeaf searchLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Search);

            BehaviourBranch failedSearch = new BehaviourBranch(searchLeaf, idleLeaf, HasTargetCondition);

            BehaviourBranch seesTarget = new BehaviourBranch(attackLeaf, searchLeaf, SeesTargetCondition);

            BehaviourBranch hasTarget = new BehaviourBranch(seesTarget, patrolBranch, HasTargetCondition);

            _behaviourTree = new BehaviourTree(hasTarget);

            ComputeBehaviour();
        }

        private void FixedUpdate()
        {
            _behaviourMachine.Update(Time.fixedDeltaTime);
        }

        protected void StateChangeHandler(byte stateId)
        {
            _currentBehaviour = (EnemyBehaviour)stateId;
            OnBehaviourChanged?.Invoke(_currentBehaviour);
        }

        public void ComputeBehaviour()
        {
            if (_behaviourTree == null)
                return;
            _behaviourMachine.SetState(_behaviourTree.GetBehaviourId());
        }

        public void RestorePatrolStamina()
        {
            _patrolStamina = _maxPatrolStamina;
        }

        protected bool PatrolStaminaCondition()
        {
            return _patrolStamina > 0;
        }

        protected bool HasTargetCondition()
        {
            return _playerdar.HasTarget;
        }

        protected bool SeesTargetCondition()
        {
            return _playerdar.SeesTarget;
        }

        protected void HealthDeathHandler()
        {
            _behaviourTree = null;
            _behaviourMachine.ForceState((byte)EnemyBehaviour.Death);
            //ServiceLocator.Instance.GetService<IHealthService>().RemoveCharacter(_health);
        }
    }
}
