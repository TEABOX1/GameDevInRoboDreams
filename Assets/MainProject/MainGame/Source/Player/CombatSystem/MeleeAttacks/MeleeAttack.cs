using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _maxComboDelay = 1f;
        
        private int _numberOfAttacks = 0;
        private float _lastAttackTime;
        private bool _isAttacking = false;
        
        private InputController _inputController;
        
        public int NumberOfAttacks => _numberOfAttacks;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnPrimaryInput += PrimaryHandler;
        }

        private void Update()
        {
            if (Time.time - _lastAttackTime >= _maxComboDelay)
                _numberOfAttacks = 0;
        }

        private void PrimaryHandler()
        {
            //TODO: Uncomment after adding animation event
            // if (_isAttacking || !_characterController.isGrounded || _numberOfAttacks >= 3) return;
            
            _isAttacking = true;
            _lastAttackTime = Time.time;
            _numberOfAttacks++;
            
            Debug.Log(_numberOfAttacks);
            
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
        }
        
        //TODO: Add OnAttackAnimationEnd() animation event
        public void OnAttackAnimationEnd()
        {
            _isAttacking = false;

            if (_numberOfAttacks >= 3)
                _numberOfAttacks = 0;
        }
    }
}