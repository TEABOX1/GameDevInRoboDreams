using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class TestSpawner : MonoBehaviour, INavPointProvider
    {
        public event Action<int> OnEnemyDeath;

        [SerializeField] private float _spawnRadius;
        [SerializeField] private Vector3 _offset;

        [SerializeField] private int _maxSpawnCount;
        [SerializeField] private float _spawnDelay;

        //[SerializeField] private SpidersPool _enemyPool;
        [SerializeField] private SpiderEnemyController _enemyController;
        //[SerializeField] private NecroEnemyController _enemyController;

        private List<EnemyController> _enemies;

        private Vector3 _point;
        private NavMeshHit _hit;

        private float _time;

        private IHealthService _healthService;

        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _enemies = new List<EnemyController>(_maxSpawnCount);

            _time = 0f;
            SpawnEnemies(_maxSpawnCount);
        }

        private void Update()
        {
            if (_time < _spawnDelay)
            {
                _time += Time.deltaTime;
                return;
            }

            _time = 0f;

            SpawnEnemies(_maxSpawnCount - _enemies.Count);
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; ++i)
                SpawnEnemy();
        }

        private void GetPointInternal()
        {
            Vector3 center = transform.position + _offset;
            Vector2 randomInCircle = Random.insideUnitCircle * _spawnRadius;
            _point.x = randomInCircle.x + center.x;
            _point.y = center.y;
            _point.z = randomInCircle.y + center.z;
            NavMesh.SamplePosition(_point, out _hit, 1.0f, NavMesh.AllAreas);
        }

        public Vector3 GetPoint()
        {
            GetPointInternal();
            return _hit.position;
        }

        private void SpawnEnemy()
        {
            GetPointInternal();

            while (!_hit.hit)
            {
                GetPointInternal();
            }

            var enemy = Instantiate(_enemyController, _hit.position, Quaternion.identity);
            Debug.Log("1");
            enemy.Initialize(this);

            _healthService.AddCharacter(enemy.Health);
            enemy.Health.OnDeath += () => EnemyDeathHandler(enemy);

            _enemies.Add(enemy);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 center = transform.position + _offset;

            Gizmos.DrawWireSphere(center, _spawnRadius);

            Gizmos.color = _hit.hit ? Color.blue : Color.red;

            Gizmos.DrawSphere(_hit.hit ? _hit.position : _point, 0.33f);
        }

        private void EnemyDeathHandler(EnemyController enemy)
        {
            _enemies.Remove(enemy);
            OnEnemyDeath?.Invoke(_enemies.Count);
        }
    }
}
