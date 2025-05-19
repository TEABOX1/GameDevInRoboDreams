using MainGame;
using System.Collections;
using UnityEngine;
using GlobalSource;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class NecroAttackAnimation : MonoBehaviour
    {
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private Animator _animator;
        //MeeleNames
        [SerializeField] private string _meeleAttackName;
        //OtherInfo
        [SerializeField] private string _idleName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;

        [SerializeField] private float _firstLockDuration;

        [SerializeField] private NecroAnimatorController _animatorController;


        private int _meeleAttackId;
        private int _idleId;

        private YieldInstruction _lockDelay;
        
        private void Start()
        {

            _lockDelay = new WaitForSeconds(_firstLockDuration);

            _meeleAttackId = Animator.StringToHash(_meeleAttackName);
            _idleId = Animator.StringToHash(_idleName);

            _attack.OnMeeleAttackStart += AttackHandler;
        }

        private void AttackHandler()
        {
            _lockDelay = new WaitForSeconds(_firstLockDuration);
            StartCoroutine(LockRoutine());
        }

        private IEnumerator LockRoutine()
        {
            PlayAttack();

            yield return _lockDelay;
            //_animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
        }

        private void LocomotionStateHandler(EnemyBehaviour state)
        {
            _enemyController.OnBehaviourChanged -= LocomotionStateHandler;
            PlayAttack();
        }

        private void PlayAttack()
        {
            //_animator.Play(_meeleAttackId, 0);
            _animator.CrossFadeInFixedTime(_meeleAttackId, _crossFadeTime);
        }
    }
}