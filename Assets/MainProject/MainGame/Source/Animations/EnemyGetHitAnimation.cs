using MainGame;
using System.Collections;
using UnityEngine;
using GlobalSource;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class EnemyGetHitAnimation : MonoBehaviour
    {
        [SerializeField] private EnemyController _plaeyrController;
        [SerializeField] private Health _health;
        [SerializeField] private Animator _animator;
        //MeeleNames
        [SerializeField] private string _getHitName;
        [SerializeField] private string _idleName;
        //OtherInfo
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;


        [SerializeField] private float _lockDuration;


        private int _getHitId;
        private int _idleId;

        private YieldInstruction _lockDelay;
        //private IInteractable _interactable;

        private void Start()
        {
            //_compositeHealth.OnDeath += PlayerDeathHandler;

            _lockDelay = new WaitForSeconds(_lockDuration);

            _getHitId = Animator.StringToHash(_getHitName);
            _idleId = Animator.StringToHash(_idleName);

            _health.OnHealthChanged += HitHandler;
        }

        private void HitHandler(int health)
        {
            StartCoroutine(LockRoutine());
        }

        private IEnumerator LockRoutine()
        {
            _animator.Play(_getHitId, 0);
            yield return _lockDelay;
            //_animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
        }
    }
}