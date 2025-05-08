using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class SpellCaster : MonoServiceBase
    {
        public override Type Type { get; } = typeof(SpellCaster);

        [SerializeField] private SpellDamageDealer _spellDamageDealer;
        [SerializeField] private SpellData _spellData;
        [SerializeField] private Transform _spawnPoint;

        private InputController _inputController;
        private float _lastCastTime = -Mathf.Infinity;
        
        public SpellData SpellData => _spellData;
        
        public void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnSecondaryInput += CastSpell;
        }

        private void CastSpell()
        {
            if (Time.time < _lastCastTime + _spellData.CooldownTime)
            {
                Debug.Log("Spell is on cooldown.");
                return;
            }
            
            if (_spellData.SpellPrefab == null)
            {
                Debug.LogWarning("Spell prefab not assigned in SpellData!");
                return;
            }
            
            SpellBase spell = Instantiate(_spellData.SpellPrefab, _spawnPoint.position, _spawnPoint.rotation);
            spell.Initialize(_spawnPoint.forward, _spellData.Speed, _spellDamageDealer);
            
            _lastCastTime = Time.time;
        }
    }
}