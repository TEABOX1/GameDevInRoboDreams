using System;
using Cinemachine;
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
        [SerializeField] private float _aimSpeed;
        
        [SerializeField] private CinemachineMixingCamera _mixingCamera;
        [SerializeField] private GameObject _crosshair;
        [SerializeField] private Camera _camera;

        private InputController _inputController;
        private float _lastCastTime = -Mathf.Infinity;
        
        private bool _isOnCooldown = false;
        
        private float _aimValue;
        private float _targetAimValue;
        
        public SpellData SpellData => _spellData;
        
        public void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnSecondaryInput += CastSpell;
            
            _crosshair.SetActive(false);
        }

        private void Update()
        {
            _aimValue = Mathf.MoveTowards(_aimValue, _targetAimValue, _aimSpeed * Time.deltaTime);
            
            _mixingCamera.m_Weight0 = 1f - _aimValue;
            _mixingCamera.m_Weight1 = _aimValue;
        }

        private void CastSpell(bool performed)
        {
            if (performed)
            {
                if (Time.time < _lastCastTime + _spellData.CooldownTime)
                {
                    Debug.Log("Spell is on cooldown.");
                    return;
                }
                
                _isOnCooldown = false;
                
                _targetAimValue = 1f;
                _crosshair.SetActive(true);
            }
            else
            {
                if (_isOnCooldown)
                {
                    Debug.Log("Spell was on cooldown during press, so skip release.");
                    return;
                }
                
                _targetAimValue = 0f;
                _crosshair.SetActive(false);
                
                if (_spellData.SpellPrefab == null)
                {
                    Debug.LogWarning("Spell prefab not assigned in SpellData!");
                    return;
                }
                
                Vector3 direction = _camera.transform.forward;
                
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, 
                        out RaycastHit hit, 100f))
                {
                    direction = (hit.point - _spawnPoint.position).normalized;
                }
                
                SpellBase spell = Instantiate(_spellData.SpellPrefab, _spawnPoint.position, _spawnPoint.rotation);
                spell.Initialize(direction, _spellData.Speed, _spellDamageDealer);

                _lastCastTime = Time.time;
                _isOnCooldown = true;
            }
        }
    }
}