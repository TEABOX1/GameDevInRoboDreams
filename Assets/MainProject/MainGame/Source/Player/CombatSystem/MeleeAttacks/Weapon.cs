using System;
using UnityEngine;

namespace MainGame
{
    public class Weapon : MonoBehaviour
    {
        public event Action<Collider> OnHit;
        
        [SerializeField] private Collider _collider;
        
        public Collider WeaponCollider => _collider;

        private void OnTriggerEnter(Collider other)
        {
            OnHit?.Invoke(other);
        }
    }
}