using UnityEngine;

namespace MainGame
{
    public class NecroAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyController _enemy;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;
        [Space, Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _movementName;
        [SerializeField] private string _meeleAttackName;
        [SerializeField] private string _fireballAttackName;
        [SerializeField] private string _spiderSpawnName;
        [SerializeField] private string _deathName;
        //[SerializeField] private string _meeleName;

        [Space, Header("Parameters")]
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;

        private int _idleId;
        private int _movementId;
        //
        private int _meeleAttackId;
        private int _fireballAttackId;
        private int _spiderSpawnId;
        //
        private int _deathId;
        private int _meeleId;

        private Vector2 _movementValue;
        
        private int _horizontalId;
        private int _verticalId;

        private bool _isMeeleAttack;
        private bool _isSpellCast;
        private bool _isDeath;


        public void SetAttackLock(bool isLocked)
        {
            _isMeeleAttack = isLocked;
        }

        public void SetSpellCastLock(bool isLocked)
        {
            _isSpellCast = isLocked;
        }
        public void SetDeathLock(bool isLocked)
        {
            _isDeath = isLocked;
        }

        private void Awake()
        {
            _idleId = Animator.StringToHash(_idleName);
            _movementId = Animator.StringToHash(_movementName);
            //
            _meeleAttackId = Animator.StringToHash(_meeleAttackName);
            _fireballAttackId = Animator.StringToHash(_fireballAttackName);
            _spiderSpawnId = Animator.StringToHash(_spiderSpawnName);
            //
            _deathId = Animator.StringToHash(_deathName);

            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);

            _enemy.OnBehaviourChanged += BehaviourStateHandler;
            _enemy.OnAttackStateChanged += AttackStateHandler;
        }

        private void AttackStateHandler(IEnemyController.AttackState state)
        {
            switch (state)
            {
                case IEnemyController.AttackState.Approach:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    break;
                case IEnemyController.AttackState.Attack:
                    //Fixed _animator.CrossFadeInFixedTime(_meeleAttackId)
                    _animator.CrossFadeInFixedTime(_meeleAttackId, _crossFadeTime);
                    break;
            }
        }
        private void BehaviourStateHandler(EnemyBehaviour state)
        {
            Debug.Log("Behaviour received in animation: " + state);

            if (_isMeeleAttack && state != EnemyBehaviour.Death)
                return;
            if (_isSpellCast && state != EnemyBehaviour.Death)
                return;
            if (_isDeath && state != EnemyBehaviour.Death)
                return;

            switch (state)
            {
                case EnemyBehaviour.Deciding:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
                case EnemyBehaviour.Idle:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
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
                //    //_animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                //    _movementValue = Vector2.zero;
                //    break;
                case EnemyBehaviour.Death:
                    SetDeathLock(true);
                    _animator.Play(_deathId);
                    _movementValue = Vector2.zero;
                    break;
            }
        }

        private void MeeleHit(Collider collider)
        {
            _animator.CrossFadeInFixedTime(_meeleId, _crossFadeTime);
        }

        public void SetAproachingAnimation()
        {
            _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
            _movementValue = Vector2.up;
        }

        public void SetShootAnimation()
        {
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            _movementValue = Vector2.zero;
        }

        public void SetMeeleAnimation()
        {
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            _movementValue = Vector2.zero;
        }

        private void Update()
        {
            _animator.SetFloat(_horizontalId, _movementValue.x, _dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalId, _movementValue.y, _dampTime, Time.deltaTime);
        }
    }
}