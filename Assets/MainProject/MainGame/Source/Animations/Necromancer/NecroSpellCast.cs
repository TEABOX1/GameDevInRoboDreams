using MainGame;
using System.Collections;
using UnityEngine;
using System;
using GlobalSource;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class NecroSpellCastAnimation : MonoBehaviour
    {
        public event Action OnFireballAnimationFinished; //added for animation
        public event Action OnSpiderAnimationFinished; //added for animation

        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private EnemySpellCaster _fireBallCast;
        [SerializeField] private SpiderSpawnSpell _spiderSpawnCast;
        [SerializeField] private Animator _animator;
        //MeeleNames
        [SerializeField] private string _fireballCastName;
        [SerializeField] private string _spiderSpawnCastName;
        //OtherInfo
        [SerializeField] private string _idleName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;

        [SerializeField] private float _fireballLockDuration;
        [SerializeField] private float _spiderSpawnLockDuration;

        [SerializeField] private NecroAnimatorController _animatorController;

        private int _fireballCastId;
        private int _spiderSpawnCastId;
        private int _idleId;

        private YieldInstruction _lockDelay;

        private void Start()
        {
            _fireballCastId = Animator.StringToHash(_fireballCastName);
            _spiderSpawnCastId = Animator.StringToHash(_spiderSpawnCastName);
            _idleId = Animator.StringToHash(_idleName);

            _fireBallCast.OnFireballCast += FireballHandler;
            _spiderSpawnCast.OnSpiderSpellCast += SpiderSpawnHandler;
        }

        private void FireballHandler()
        {
            _lockDelay = new WaitForSeconds(_fireballLockDuration);
            StartCoroutine(FireballLockRoutine());
        }
        private void SpiderSpawnHandler()
        {
            _lockDelay = new WaitForSeconds(_fireballLockDuration);
            StartCoroutine(SpiderSpawnLockRoutine());
        }

        private IEnumerator FireballLockRoutine()
        {
            _animatorController.SetSpellCastLock(true);
            _animator.Play(_fireballCastId, 0);
            //_animator.CrossFadeInFixedTime(_fireballCastId, _crossFadeTime);
            yield return _lockDelay;
            //_animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            OnFireballAnimationFinished?.Invoke();
            _animatorController.SetSpellCastLock(false);
        }

        private IEnumerator SpiderSpawnLockRoutine()
        {
            _animatorController.SetSpellCastLock(true);
            _animator.Play(_spiderSpawnCastId, 0);
            //_animator.CrossFadeInFixedTime(_spiderSpawnCastId, _crossFadeTime);
            yield return _lockDelay;
            //_animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            OnSpiderAnimationFinished?.Invoke();
            _animatorController.SetSpellCastLock(false);
        }
    }
}