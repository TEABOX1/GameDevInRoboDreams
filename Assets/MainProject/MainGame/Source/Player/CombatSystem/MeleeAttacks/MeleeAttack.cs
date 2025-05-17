using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class MeleeAttack : MonoBehaviour
    {
        public event Action<int> OnAttack; 
        
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _maxComboDelay = 1f;

        [SerializeField] private AttackAnimation _attackAnimation;

        private int _numberOfAttacks = 0;
        private float _lastAttackTime;
        private bool _isAttacking = false;
        
        private InputController _inputController;
        
        public int NumberOfAttacks => _numberOfAttacks;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnPrimaryInput += PrimaryHandler;

            _attackAnimation.OnAttackAnimationEnd += AttackAnimationEndHandler;
        }

        private void Update()
        {
            if (Time.time - _lastAttackTime >= _maxComboDelay)
                _numberOfAttacks = 0;
        }

        private void PrimaryHandler()
        {
            if (_isAttacking || !_characterController.isGrounded || _numberOfAttacks >= 3) return;
            
            _isAttacking = true;
            _lastAttackTime = Time.time;
            _numberOfAttacks++;

            // Debug.Log(_numberOfAttacks);
            
            switch (_numberOfAttacks)
            {
                case 1:
                    Debug.Log("First attack");
                    break;
                case 2:
                    Debug.Log("Second attack");
                    break;
                case 3:
                    Debug.Log("Third attack");
                    break;
            }
            OnAttack?.Invoke(_numberOfAttacks);
        }
        
        private void AttackAnimationEndHandler()
        {
            Debug.Log(_numberOfAttacks);
            _isAttacking = false;

            if (_numberOfAttacks >= 3)
                _numberOfAttacks = 0;
        }
    }
}