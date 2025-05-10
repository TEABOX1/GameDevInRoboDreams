using GlobalSource;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class AttackBehaviour : BehaviourStateBase
    {
        public enum AttackState
        {
            Approach = 0,
            Attack =1,

            NullState = 255
        }

        public event Action<AttackState> OnAttackStateChange;

        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly EnemyAttack _attackController;
        private readonly Transform _characterTransform;
        private readonly Transform _targetTransform;

        private float _time;
        private float _distance;

        private AttackState _currentState;
        public AttackState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;
                _currentState = value;
            }
        }

        public AttackBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
            _targetTransform = enemyController.PlayerRadar.CurrentTarget;
            _attackController = enemyController.EnemyAttack;
        }

        public override void Enter()
        {
            base.Enter();
            _time = 0f;
            _currentState = AttackState.Approach;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            UpdateRotation();

            switch (_currentState)
            {
                case AttackState.Approach:
                    ApproachUpdate(deltaTime);
                    break;
                case AttackState.Attack:
                    AttackUpdate(deltaTime);
                    break;
            }
        }

        private void ApproachUpdate(float deltaTime)
        {
            _time += deltaTime;
            _distance = Vector3.Distance(_targetTransform.position, _characterTransform.position);

            if (_distance <= _attackController.AttackData.Distance)
            {
                _agent.isStopped = true;
                //_currentState = AttackState.Attack;
                ChangeState(AttackState.Attack);
                return;
            }

            _agent.isStopped = false;

            _agent.stoppingDistance = _attackController.AttackData.Distance;
            _agent.SetDestination(_targetTransform.position);

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0f;

            _characterController.Move(velocity * (deltaTime * enemyController.Data.PatrolSpeed) + Physics.gravity);

            Vector3 newPosition = _characterTransform.position;
            Vector3 direction = newPosition - _characterTransform.position;

            _agent.nextPosition = _characterTransform.position;

            float remainingDistance = _agent.remainingDistance;
            if (!_agent.pathPending && remainingDistance <= _attackController.AttackData.Distance)
            {
                //_currentState = AttackState.Attack;
                ChangeState(AttackState.Attack);
            }
        }

        private void AttackUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < _attackController.AttackData.Interval)
                return;

            //to-do: викликати анімацію атаки
            _attackController.Attack();

            _distance = Vector3.Distance(_targetTransform.position, _characterTransform.position);
            _time = 0f;

            if (_distance > _attackController.AttackData.Distance)
            {
                //_currentState = AttackState.Approach;
                ChangeState(AttackState.Approach);
            }
        }

        public override void Exit()
        {
            base.Exit();

            _agent.isStopped = false;
            _agent.ResetPath();
            _agent.stoppingDistance = 0f;
        }

        public override void Dispose()
        {
        }

        protected void ChangeState(AttackState state)
        {
            CurrentState = state;
            OnAttackStateChange?.Invoke(CurrentState);
        }

        private void UpdateRotation()
        {
            Vector3 direction = Vector3
                .ProjectOnPlane(
                    enemyController.PlayerRadar.CurrentTarget.position - _characterTransform.position,
                    Vector3.up).normalized;

            _characterTransform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
