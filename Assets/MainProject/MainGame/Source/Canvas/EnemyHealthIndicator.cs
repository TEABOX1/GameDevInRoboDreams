using GlobalSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EnemyHealthIndicator : MonoBehaviour
    {
        [SerializeField] GameObject _bossHealthCanvas;
        [SerializeField] private RectTransform _healthValue;
        [SerializeField] private Vector2 _referenceSize;
        [SerializeField] private float _damageDecaySpeed;
        [SerializeField] private float _regenerationSpeed;
        [SerializeField] private BossFightArea _bossSpawner;

        private float _targetHealth;
        private float _displayedHealth;
        private float _displayedDamage;

        private EnemyService _enemyService;
        private IHealth _bossHealth;

        private void Start()
        {
            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();

            _enemyService.OnBossDefeated += BossDeathHandler;

            _bossSpawner.OnBossSpawn += BossSpawnHandler;

            _bossHealthCanvas.SetActive(false);
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

        private void BossSpawnHandler(IHealth bossHealth)
        {
            _bossHealthCanvas.SetActive(true);
            _bossHealth = bossHealth;
            ForceHealth(_bossHealth.HealthValue);
            _bossHealth.OnHealthChanged += HealthChangedHandler;
        }

        private void BossDeathHandler()
        {
            StartCoroutine(DisableBossCanvasWithDelay());
        }

        private IEnumerator DisableBossCanvasWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _bossHealthCanvas.SetActive(false);
        }
    }
}