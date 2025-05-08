using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layerMask;
        
        protected bool _canDealDamage;
        //TODO: Change when healthSystem is added
        protected HashSet</*Health*/ Collider> _hasDealtDamage;

        protected virtual void Start()
        {
            _canDealDamage = false;
            _hasDealtDamage = new HashSet</*Health*/ Collider>();
        }
        
        private void Update()
        {}
    }
}