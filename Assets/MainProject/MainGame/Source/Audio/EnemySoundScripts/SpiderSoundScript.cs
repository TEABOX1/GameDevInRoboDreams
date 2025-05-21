using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class SpiderSoundScript : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _movementClip;
        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _dieClip;

        [Header("Spider Data")]
        [SerializeField] EnemyController _spellCaster;

        [Header("Settings")]
        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            _spellCaster.OnBehaviourChanged += BehaviourStateHandler;
            _spellCaster.OnAttackStateChanged += AttaStateHandler;
            _audioSource.Stop();
        }

        private void AttaStateHandler(IEnemyController.AttackState state)
        {
            switch (state)
            {
                case IEnemyController.AttackState.Approach:
                    _audioSource.Pause();
                    _audioSource.clip = _movementClip;
                    _audioSource.Play();
                    break;
                case IEnemyController.AttackState.Attack:
                    _audioSource.Pause();
                    _audioSource.clip = _attackClip;
                    _audioSource.Play();
                    break;
            }
        }

        private void BehaviourStateHandler(EnemyBehaviour state)
        {
            switch (state)
            {
                case EnemyBehaviour.Deciding:
                    _audioSource.Pause();
                    _audioSource.clip = _movementClip;
                    _audioSource.Play();
                    break;
                case EnemyBehaviour.Idle:
                    _audioSource.Pause();
                    _audioSource.clip = _movementClip;
                    _audioSource.Play();
                    break;
                case EnemyBehaviour.Patrol:
                    _audioSource.Pause();
                    _audioSource.clip = _movementClip;
                    _audioSource.Play();
                    break;
                case EnemyBehaviour.Search:
                    _audioSource.Pause();
                    _audioSource.clip = _movementClip;
                    _audioSource.Play();
                    break;
                case EnemyBehaviour.Death:
                    _audioSource.Pause();
                    _audioSource.clip = _dieClip;
                    _audioSource.loop = false;
                    _audioSource.Play();
                    break;
            }
        }

    }
}
