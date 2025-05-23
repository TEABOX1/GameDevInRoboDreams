using System.Collections;
using UnityEngine;
using GlobalSource;
using System;
using Boot;
using UnityEngine.InputSystem.XInput;

namespace MainGame
{
    public class PlayerDeath : MonoBehaviour
    {
        public event Action OnPlayerDeath;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _deathName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _healthBarDelayTime;
        [SerializeField] private GameObject _logicalPlayer;

        [SerializeField] private PlayerAnimatorController _playerAnimation;

        private IHealth _health;

        private void Start()
        {
            _health = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<IHealth>();
            _health.OnDeath += DeathHandler;
        }

        private void DeathHandler()
        {
            OnPlayerDeath?.Invoke();
            ServiceLocator.Instance.GetService<InputController>().DefaulMapLock();
            ServiceLocator.Instance.GetService<InputController>().CursorEnable();
        }

        private IEnumerator DelayedDestroy()
        {
            //_logicalPlayer.SetActive(false);
            //ServiceLocator.Instance.GetService<IGameStateProvider>()?.SetGameState(GameState.Paused);
            //yield return null;

            _animator.CrossFadeInFixedTime(_deathName, _crossFadeTime);
            //_handsIK.DisableIK();

            yield return new WaitForSeconds(_healthBarDelayTime);
            //ServiceLocator.Instance.GetService<IGameStateProvider>()?.SetGameState(GameState.Paused);

            //ServiceLocator.Instance.GetService<ISaturationService>().SetDeathSaturation();
        }
    }
}