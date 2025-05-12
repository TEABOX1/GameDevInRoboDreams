using GlobalSource;
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
        protected HashSet<IHealth> _hasDealtDamage;

        protected IHealthService _healthService;

        protected virtual void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();

            _canDealDamage = false;
            _hasDealtDamage = new HashSet<IHealth> ();
        }

        private void Update()
        { }
    }
}