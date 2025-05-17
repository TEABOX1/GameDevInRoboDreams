using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace MainGame
{
    public class SpiderAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyController _enemy;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;
        [Space, Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _movementName;
        [SerializeField] private string _deathName;
        //[SerializeField] private string _meeleName;

        [Space, Header("Parameters")]
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;
        [SerializeField] private string _animationValueName;

        private int _idleId;
        private int _movementId;
        private int _deathId;

        private Vector2 _movementValue;
        
        private int _horizontalId;
        private int _verticalId;
        private int _animationValueId;

        float[] _animationValueForIdle = { 0f, 0.5f, 1f };
        float[] _animationValueForDeath = { 0f, 1f };

        private bool _isAttacking;
        private bool _isDead;
        private bool _isHit;

        public void SetAttackLock(bool isLocked)
        {
            _isAttacking = isLocked;
        }
        public void SetDeathLock(bool isLocked)
        {
            _isDead = isLocked;
        }
        public void SetHitLock(bool isLocked)
        {
            _isHit = isLocked;
        }

        private void Awake()
        {
            _idleId = Animator.StringToHash(_idleName);
            _movementId = Animator.StringToHash(_movementName);
            _deathId = Animator.StringToHash(_deathName);

            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);
            _animationValueId = Animator.StringToHash(_animationValueName);

            _enemy.OnBehaviourChanged += BehaviourStateHandler;
            _enemy.OnAttackStateChanged += AttaStateHandler;
        }

        private void AttaStateHandler(IEnemyController.AttackState state)
        {
            switch (state)
            {
                case IEnemyController.AttackState.Approach:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
                case IEnemyController.AttackState.Attack:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
            }
        }

        private void BehaviourStateHandler(EnemyBehaviour state)
        {
            if (_isAttacking && state != EnemyBehaviour.Death)
                return;
            if (_isDead && state != EnemyBehaviour.Death)
                return;

            switch (state)
            {
                case EnemyBehaviour.Deciding:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
                case EnemyBehaviour.Idle:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _animator.SetFloat(_animationValueId, _animationValueForIdle[Random.Range(0, _animationValueForIdle.Length)], _dampTime, Time.deltaTime);
                    _movementValue = Vector2.zero;
                    break;
                case EnemyBehaviour.Patrol:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    break;
                case EnemyBehaviour.Search:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    break;
                //case EnemyBehaviour.Attack:
                //    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                //    _movementValue = Vector2.zero;
                //    break;
                case EnemyBehaviour.Death:
                    SetDeathLock(true);
                    _animator.CrossFadeInFixedTime(_deathId, _crossFadeTime);
                    _animator.SetFloat(_animationValueId, _animationValueForDeath[Random.Range(0, _animationValueForDeath.Length)], _dampTime, Time.deltaTime);
                    _movementValue = Vector2.zero;
                    break;
            }
        }

        private void Update()
        {
            _animator.SetFloat(_horizontalId, _movementValue.x, _dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalId, _movementValue.y, _dampTime, Time.deltaTime);
        }
    }
}