using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class NecromancerCastScript : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _fireballClip;
        [SerializeField] private AudioClip _summonClip;
        [SerializeField] private AudioClip _attackClip;

        [Header("Necro Data")]
        [SerializeField] NecroSpellCastAnimation _spellCaster;
        [SerializeField] NecroAttackAnimation _attackAnimation;

        [Header("Settings")]
        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource.Stop();
            _spellCaster.OnFireballAnimationFinished += FireballCastHandler;
            _spellCaster.OnSpiderAnimationFinished += SummonCastHandler;

            _attackAnimation.OnAttackStarted += AttackHandler;
        }

        private void FireballCastHandler()
        {
            _audioSource.Stop();
            _audioSource.clip = _fireballClip;
            _audioSource.Play();
        }

        private void SummonCastHandler()
        {
            _audioSource.Stop();
            _audioSource.clip = _summonClip;
            _audioSource.Play();
        }

        private void AttackHandler()
        {
            _audioSource.Stop();
            _audioSource.clip = _attackClip;
            _audioSource.Play();
        }

    }
}
