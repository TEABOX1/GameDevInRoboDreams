using GlobalSource;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class NecroAttackBehaviour : BehaviourStateBase
    {
        public enum AttackState
        {
            Approach = 0,
            Attack =1,

            NullState = 255
        }

        public enum AttackMode
        {
            Melee = 0,
            Ranged = 1,

            NullState = 255
        }

        public event Action<AttackState> OnAttackStateChange;
        public event Action<AttackMode> OnAttackModeChange;
        public event Action<Collider> OnHit;

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

        private AttackMode _currentMode;
        public AttackMode CurrentMode
        {
            get => _currentMode;
            set
            {
                if (_currentMode == value)
                    return;
                _currentMode = value;
            }
        }

        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly EnemyAttack _attackController;
        private readonly EnemySpellCaster _enemySpellCaster;
        private readonly SpiderSpawnSpell _spiderSpawnSpell;

        private readonly Transform _characterTransform;
        private readonly Transform _targetTransform;
        private readonly float _switchToMeleeDistance;
        private readonly float _switchToRangedDistance;

        private float _attackTimer;
        private float _spellTimer;
        private float _spawnTimer;
        private float _distance;

        public NecroAttackBehaviour(StateMachine stateMachine, byte stateId, NecroEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _attackController = enemyController.EnemyAttack;
            _enemySpellCaster = enemyController.EnemySpellCaster;
            _spiderSpawnSpell = enemyController.SpiderSpawnSpell;

            _characterTransform = enemyController.CharacterTransform;
            _targetTransform = enemyController.PlayerRadar.CurrentTarget;
            _switchToMeleeDistance = enemyController.ToMeleeDistance;
            _switchToRangedDistance = enemyController.ToRangedDistance;

        }

        public override void Enter()
        {
            base.Enter();
            _attackTimer = 0f;
            _spellTimer = 0f;
            _spawnTimer = 0f;

            _currentMode = AttackMode.Ranged;
            _currentState = AttackState.Attack;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _distance = Vector3.Distance(_targetTransform.position, _characterTransform.position);
            if (_currentMode == AttackMode.Ranged && _distance <= _switchToMeleeDistance)
            {
                SetAttackMode(AttackMode.Melee);
                ChangeState(AttackState.Approach);
            }
            else if (_currentMode == AttackMode.Melee && _distance >= _switchToRangedDistance)
            {
                SetAttackMode(AttackMode.Ranged);
                ChangeState(AttackState.Attack);
            }

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
            UpdateTimers(deltaTime);

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
            UpdateTimers(deltaTime);

            if (_currentMode == AttackMode.Melee)
            {
                if (_attackTimer < _attackController.AttackData.Interval)
                    return;

                //to-do: викликати анімацію атаки
                _attackController.Attack();

                _distance = Vector3.Distance(_targetTransform.position, _characterTransform.position);
                _attackTimer = 0f;

                if (_distance > _attackController.AttackData.Distance)
                {
                    //_currentState = AttackState.Approach;
                    ChangeState(AttackState.Approach);
                }
            }
            else
            {
                if (_spellTimer < _enemySpellCaster.SpellData.CooldownTime && _spawnTimer < _spiderSpawnSpell.SpawnSpellCooldown)
                    return;

                if (_spawnTimer >= _spiderSpawnSpell.SpawnSpellCooldown)
                {
                    //to-do: викликати анімацію спавну павуків
                    _spiderSpawnSpell.SpawnSpiders();
                    _spawnTimer = 0f;
                    //return;
                }
                else if (_spellTimer >= _enemySpellCaster.SpellData.CooldownTime)
                {
                    //to-do: викликати анімацію касту фаерболу
                    _enemySpellCaster.CastSpell(_targetTransform);
                    _spellTimer = 0f;
                }
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

        private void SetAttackMode(AttackMode mode)
        {
            CurrentMode = mode;
            OnAttackModeChange?.Invoke(CurrentMode);
        }

        private void UpdateRotation()
        {
            Vector3 direction = Vector3
                .ProjectOnPlane(
                    enemyController.PlayerRadar.CurrentTarget.position - _characterTransform.position,
                    Vector3.up).normalized;

            _characterTransform.rotation = Quaternion.LookRotation(direction);
            //можливо треба буде повертати також зброю (посох)
        }

        private void UpdateTimers(float deltaTime)
        {
            _attackTimer += deltaTime;
            _spellTimer += deltaTime;
            _spawnTimer += deltaTime;
        }
    }
}
