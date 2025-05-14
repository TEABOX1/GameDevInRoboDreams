using MainGame;
using System.Collections;
using UnityEngine;
using GlobalSource;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class SpiderAttackAnimation : MonoBehaviour
    {
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private Animator _animator;
        //MeeleNames
        [SerializeField] private string _meeleAttackName;
        [SerializeField] private string _animationValueName;
        //OtherInfo
        [SerializeField] private string _idleName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;

        [SerializeField] private float _firstLockDuration;


        private int _meeleAttackId;
        private int _idleId;

        private YieldInstruction _lockDelay;

        private int _animationValueId;

        float[] _animationValue = { 0f, 0.5f, 1f };

        private void Start()
        {

            _lockDelay = new WaitForSeconds(_firstLockDuration);

            _meeleAttackId = Animator.StringToHash(_meeleAttackName);
            _idleId = Animator.StringToHash(_idleName);

            _animationValueId = Animator.StringToHash(_animationValueName);

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
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
        }

        private void PlayAttack()
        {
            _animator.SetFloat(_animationValueId, _animationValue[Random.Range(0, _animationValue.Length)], _dampTime, Time.deltaTime);
            _animator.CrossFadeInFixedTime(_meeleAttackId, _crossFadeTime);
        }
    }
}