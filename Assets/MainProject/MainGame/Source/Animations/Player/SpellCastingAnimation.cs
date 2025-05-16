using MainGame;
using System.Collections;
using UnityEngine;
using GlobalSource;
using UnityEngine.InputSystem;
using Tiny;

namespace MainGame
{
    public class SpellCastingAnimation : MonoBehaviour
    {
        [SerializeField] private PlayerController _plaeyrController;
        [SerializeField] private SpellCaster _attack;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _sword;
        [SerializeField] private Trail _leftHand;
        [SerializeField] private Trail _rightHand;
        //[SerializeField] private HandsIK _handsIK;
        //MeeleNames
        [SerializeField] private string _magicAttackCastingName;
        [SerializeField] private string _magicAttackStopCastingName;
        //OtherInfo
        [SerializeField] private string _idleName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;


        [SerializeField] private float _firstLockDuration;


        private int _magicAttackCastingId;
        private int _magicAttackStopCastingId;
        private int _idleId;

        private YieldInstruction _lockDelay;
        //private IInteractable _interactable;

        private InputController _inputController;
        private IHealth _compositeHealth;

        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();

            //_compositeHealth = ServiceLocator.Instance.GetService<IPlayerService>().Player
            //    .GetComponent<IHealth>();

            //_compositeHealth.OnDeath += PlayerDeathHandler;

            _lockDelay = new WaitForSeconds(_firstLockDuration);

            _magicAttackCastingId = Animator.StringToHash(_magicAttackCastingName);
            _magicAttackStopCastingId = Animator.StringToHash(_magicAttackStopCastingName);
            _idleId = Animator.StringToHash(_idleName);

            _attack.OnSpellCast += SpellHandler;
        }

        private void SpellHandler(bool isCasting)
        {
            if (isCasting)
            {
                //_leftHand.enabled = true;
                //_rightHand.enabled = true;
                _sword.SetActive(false);
                _animator.CrossFadeInFixedTime(_magicAttackCastingId, _crossFadeTime, 1);
            }
            else
                StartCoroutine(LockRoutine());
        }

        private IEnumerator LockRoutine()
        {
            _inputController.DefaulMapLock();
            if (_plaeyrController.PlayerControllerState != PlayerControllerState.Idle)
            {
                _plaeyrController.OnStateChanged += LocomotionStateHandler;
            }
            else
            {
                PlayCastFinish();
            }

            yield return _lockDelay;
            _inputController.DefaultMapUnlock();
            //_animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            //_leftHand.enabled = false;
            //_rightHand.enabled = false;
            _sword.SetActive(true);
        }

        private void LocomotionStateHandler(PlayerControllerState state)
        {
            _plaeyrController.OnStateChanged -= LocomotionStateHandler;
            PlayCastFinish();
        }

        private void PlayCastFinish()
        {
            _animator.CrossFadeInFixedTime(_magicAttackStopCastingId, _crossFadeTime, 1);
        }

        private void PlayerDeathHandler()
        {
            StopAllCoroutines();
        }
    }
}