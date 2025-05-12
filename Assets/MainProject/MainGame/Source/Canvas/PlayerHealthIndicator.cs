using UnityEngine;

namespace MainGame
{
    public class PlayerHealthIndicator : MonoBehaviour
    {
        [SerializeReference] private Health _health;
        [SerializeField] private RectTransform _healthValue;
        [SerializeField] private Vector2 _referenceSize;
        [SerializeField] private float _damageDecaySpeed;
        [SerializeField] private float _regenerationSpeed;
        
        private float _targetHealth;
        private float _displayedHealth;
        private float _displayedDamage;
        
        private void Start()
        {
            ForceHealth(_health.HealthValue);
            Debug.Log("Health: " + _health.HealthValue);
            _health.OnHealthChanged += HealthChangedHandler;
        }

        private void Update()
        {
            if (_targetHealth < _displayedHealth)
                _displayedHealth = _targetHealth;
            else
            {
                _displayedHealth =
                    Mathf.MoveTowards(_displayedHealth, _targetHealth,
                    _regenerationSpeed * Time.deltaTime);
            }

            if (_displayedDamage > _displayedHealth)
            {
                _displayedDamage =
                    Mathf.MoveTowards(_displayedDamage, _displayedHealth,
                        _damageDecaySpeed * Time.deltaTime);
            }
            else
                _displayedDamage = _displayedHealth;
            
            _healthValue.sizeDelta = new Vector2(_referenceSize.x * _displayedHealth, _referenceSize.y);
        }

        private void HealthChangedHandler(int health) => SetHealth(health);

        private void SetHealth(int health)
        {
            _targetHealth = health * 0.01f;
        }

        private void ForceHealth(int health)
        {
            _displayedDamage = _displayedHealth = _targetHealth = health * 0.01f;
        }
    }
}