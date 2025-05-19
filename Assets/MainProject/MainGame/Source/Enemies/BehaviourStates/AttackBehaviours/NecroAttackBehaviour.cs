using GlobalSource;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class NecroAttackBehaviour : BehaviourStateBase
    {

        public event Action<IEnemyController.AttackState> OnAttackStateChange;
        public event Action<IEnemyController.AttackMode> OnAttackModeChange;

        private IEnemyController.AttackState _currentState;
        public IEnemyController.AttackState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;
                _currentState = value;
            }
        }

        private IEnemyController.AttackMode _currentMode;
        public IEnemyController.AttackMode CurrentMode
        {
            get => _currentMode;
            set
            {
                if (_currentMode == value)
                    return;
                _currentMode = value;
            }
        }

        private EnemyService _enemyService;
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly EnemyAttack _attackController;
        private readonly EnemySpellCaster _enemySpellCaster;
        private readonly SpiderSpawnSpell _spiderSpawnSpell;
        private NecroSpellCastAnimation _necroSpellCastAnimation;

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
            _necroSpellCastAnimation = enemyController.NecroSpellCastAnimation;

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
            _spawnTimer = 18f;

            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _necroSpellCastAnimation.OnFireballAnimationFinished += ProcceedCast;

            _currentMode = IEnemyController.AttackMode.Ranged;
            _currentState = IEnemyController.AttackState.Attack;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _distance = Vector3.Distance(enemyController.PlayerRadar.CurrentTarget.position, _characterTransform.position);
            if (_currentMode == IEnemyController.AttackMode.Ranged && _distance <= _switchToMeleeDistance)
            {
                SetAttackMode(IEnemyController.AttackMode.Melee);
                ChangeState(IEnemyController.AttackState.Approach);
            }
            else if (_currentMode == IEnemyController.AttackMode.Melee && _distance >= _switchToRangedDistance)
            {
                SetAttackMode(IEnemyController.AttackMode.Ranged);
                ChangeState(IEnemyController.AttackState.Attack);
            }

            UpdateRotation();

            switch (_currentState)
            {
                case IEnemyController.AttackState.Approach:
                    ApproachUpdate(deltaTime);
                    break;
                case IEnemyController.AttackState.Attack:
                    AttackUpdate(deltaTime);
                    break;
            }
        }

        private void ApproachUpdate(float deltaTime)
        {
            UpdateTimers(deltaTime);

            _distance = Vector3.Distance(enemyController.PlayerRadar.CurrentTarget.position, _characterTransform.position);

            if (_distance <= _attackController.AttackData.Distance)
            {
                _agent.isStopped = true;
                //_currentState = AttackState.Attack;
                ChangeState(IEnemyController.AttackState.Attack);
                return;
            }

            _agent.isStopped = false;

            _agent.stoppingDistance = _attackController.AttackData.Distance;
            _agent.SetDestination(enemyController.PlayerRadar.CurrentTarget.position);

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
                ChangeState(IEnemyController.AttackState.Attack);
            }
        }

        private void AttackUpdate(float deltaTime)
        {
            UpdateTimers(deltaTime);

            if (_currentMode == IEnemyController.AttackMode.Melee)
            {
                if (_attackTimer < _attackController.AttackData.Interval)
                    return;

                _attackController.Attack();

                _distance = Vector3.Distance(enemyController.PlayerRadar.CurrentTarget.position, _characterTransform.position);
                _attackTimer = 0f;

                if (_distance > _attackController.AttackData.Distance)
                {
                    //_currentState = AttackState.Approach;
                    ChangeState(IEnemyController.AttackState.Approach);
                }
            }
            else
            {
                if (_spellTimer < _enemySpellCaster.EnemySpellData.CooldownTime && _spawnTimer < _spiderSpawnSpell.SpawnSpellCooldown)
                    return;

                if (_spawnTimer >= _spiderSpawnSpell.SpawnSpellCooldown && _enemyService.GetEnemiesOfTypeCount(EnemyTypes.SpawnSpider) == 0 /*_spiderSpawnSpell.SpawnSpidersCount == 0*/)
                {
                    _spiderSpawnSpell.SpawnSpiders();
                    _spawnTimer = 0f;
                }
                
                if (_spellTimer >= _enemySpellCaster.EnemySpellData.CooldownTime)
                {
                    _enemySpellCaster.CastSpell();
                    _spellTimer = 0f;
                }
            }
            
        }

        private void ProcceedCast()
        {
            //Vector3 point = enemyController.PlayerRadar.CurrentTarget.position + new Vector3(0f, 0.5f, 0f);
            _enemySpellCaster.ProcceedCast(enemyController.PlayerRadar.CurrentTarget.position);
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

        protected void ChangeState(IEnemyController.AttackState state)
        {
            CurrentState = state;
            Debug.Log($"Necro Attack State Change! Current State = {CurrentState}");
            OnAttackStateChange?.Invoke(CurrentState);
            enemyController.InvokeAttackState(CurrentState);
        }

        private void SetAttackMode(IEnemyController.AttackMode mode)
        {
            CurrentMode = mode;
            Debug.Log($"Necro Attack Mode Change! Current State = {CurrentMode}");
            OnAttackModeChange?.Invoke(CurrentMode);
            enemyController.InvokeAttackMode(CurrentMode);
        }

        private void UpdateRotation()
        {
            Vector3 direction = Vector3
                .ProjectOnPlane(
                    enemyController.PlayerRadar.CurrentTarget.position - _characterTransform.position,
                    Vector3.up).normalized;

            _characterTransform.rotation = Quaternion.LookRotation(direction);
        }

        private void UpdateTimers(float deltaTime)
        {
            _attackTimer += deltaTime;
            _spawnTimer += deltaTime;
            _spellTimer += deltaTime;
        }
    }
}
