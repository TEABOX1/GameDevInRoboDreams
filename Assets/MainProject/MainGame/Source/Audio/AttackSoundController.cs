using System;
using UnityEngine;

namespace MainGame
{
    public class AttackSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _attackAudioClip;
        [SerializeField] private AudioClip _doubleAttackAudioClip;
        [SerializeField] private AttackAnimation _attackAnimation;
        [SerializeField] private MeleeAttack _attack;

        public void Start()
        {
            _attackAnimation.OnAttackAnimationStart += AttackAnimationStartHandler;
        }

        private void AttackAnimationStartHandler()
        {
            switch (_attack.NumberOfAttacks)
            {
                case 1:
                case 2:
                    _audioSource.PlayOneShot(_attackAudioClip);
                    break;
                case 3:
                    _audioSource.PlayOneShot(_doubleAttackAudioClip);
                    break;
                default:
                    Debug.Log("Invalid number of attacks");
                    break;
            }
        }
        
        //Animation event, can be used instead of code above
        public void Attack()
        {
            // _audioSource.PlayOneShot(_attackAudioClip);
        }
    }
}