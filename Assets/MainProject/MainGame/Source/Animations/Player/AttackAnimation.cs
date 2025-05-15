using MainGame;
using System.Collections;
using UnityEngine;
using GlobalSource;
using UnityEngine.InputSystem;
using Tiny;
using System;

namespace MainGame
{
    public class AttackAnimation : MonoBehaviour
    {
        public event Action OnAttackAnimationEnd;

        [SerializeField] private PlayerController _plaeyrController;
        [SerializeField] private MeleeAttack _attack;
        [SerializeField] private Animator _animator;

        [SerializeField] private Trail _trailScript;
        //[SerializeField] private HandsIK _handsIK;
        //MeeleNames
        [SerializeField] private string _meeleAttack1Name;
        [SerializeField] private string _meeleAttack2Name;
        [SerializeField] private string _meeleAttack3Name;
        //OtherInfo
        [SerializeField] private string _idleName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;

        [SerializeField] private float _firstLockDuration;
        [SerializeField] private float _secondLockDuration;
        [SerializeField] private float _thirdLockDuration;


        private int _meeleAttack1Id;
        private int _meeleAttack2Id;
        private int _meeleAttack3Id;
        private int _idleId;

        float _attackNumber;

        private YieldInstruction _lockDelay;

        private InputController _inputController;
        private IHealth _Health;

        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();

            //_Health = ServiceLocator.Instance.GetService<IPlayerService>().Player
            //    .GetComponent<IHealth>();

            //_Health.OnDeath += PlayerDeathHandler;

            _lockDelay = new WaitForSeconds(_firstLockDuration);

            _meeleAttack1Id = Animator.StringToHash(_meeleAttack1Name);
            _meeleAttack2Id = Animator.StringToHash(_meeleAttack2Name);
            _meeleAttack3Id = Animator.StringToHash(_meeleAttack3Name);
            _idleId = Animator.StringToHash(_idleName);

            _attack.OnAttack += AttackHandler;
        }

        private void AttackHandler(int numberOfAttacks)
        {
            _attackNumber = numberOfAttacks;
            switch (_attackNumber)
            {
                case 1:
                    _lockDelay = new WaitForSeconds(_firstLockDuration);
                    break;
                case 2:
                    _lockDelay = new WaitForSeconds(_secondLockDuration);
                    break;
                case 3:
                    _lockDelay = new WaitForSeconds(_thirdLockDuration);
                    break;
            }
            StartCoroutine(LockRoutine());
        }

        private IEnumerator LockRoutine()
        {
            _trailScript.enabled = true;
            _inputController.DefaulMapLock();
            if (_plaeyrController.PlayerControllerState != PlayerControllerState.Idle)
            {
                _plaeyrController.OnStateChanged += LocomotionStateHandler;
            }
            else
            {
                PlayAttack();
            }
            
            yield return _lockDelay;
            _inputController.DefaultMapUnlock();
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            OnAttackAnimationEnd?.Invoke();
            _trailScript.enabled = false;
        }

        private void LocomotionStateHandler(PlayerControllerState state)
        {
            _plaeyrController.OnStateChanged -= LocomotionStateHandler;
            PlayAttack();
        }

        private void PlayAttack()
        {
            switch(_attackNumber)
            {
                case 1:
                    _animator.CrossFadeInFixedTime(_meeleAttack1Id, _crossFadeTime);
                    break;
                case 2:
                    _animator.CrossFadeInFixedTime(_meeleAttack2Id, _crossFadeTime);
                    break;
                case 3:
                    _animator.CrossFadeInFixedTime(_meeleAttack3Id, _crossFadeTime);
                    break;
            }
        }

        private void PlayerDeathHandler()
        {
            StopAllCoroutines();
        }
    }
}