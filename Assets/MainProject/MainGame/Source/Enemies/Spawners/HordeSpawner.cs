using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class HordeSpawner : MonoBehaviour, INavPointProvider
    {
        public event Action OnQuestCompleted;

        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected float _spawnRadius;
        [SerializeField] protected Vector3 _offset;

        [SerializeField] protected int _spawnCount;

        protected List<EnemyController> _enemies = new();

        protected Vector3 _point;
        protected NavMeshHit _hit;

        protected IHealthService _healthService;

        public int EnemyCount => _enemies.Count;

        protected virtual void Awake()
        {
            enabled = false;
            //підписка на подію старту квеста
            // ... += QuestStartHandler();
        }

        protected virtual void OnEnable()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            SpawnEnemies(_spawnCount);
        }

        protected virtual void Update()
        {
            if (EnemyCount == 0)
            {
                OnQuestCompleted?.Invoke();
                enabled = false;
            }
        }

        protected void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; ++i)
                SpawnEnemy();
        }

        protected virtual void SpawnEnemy()
        {
            GetPointInternal();

            int depth = 0;
            while (!_hit.hit)
            {
                GetPointInternal();
                depth++;
                if (depth > 100000)
                {
                    Debug.LogError("Point sampling reached 100000 iterations, aborting");
                    return;
                }
            }
        }

        public Vector3 GetPoint()
        {
            GetPointInternal();
            return _hit.position;
        }

        protected void GetPointInternal()
        {
            Vector3 center = transform.position + _offset;
            Vector2 randomInCircle = Random.insideUnitCircle * _spawnRadius;
            _point.x = randomInCircle.x + center.x;
            _point.y = center.y;
            _point.z = randomInCircle.y + center.z;
            NavMesh.SamplePosition(_point, out _hit, 1.0f, NavMesh.AllAreas);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 center = transform.position + _offset;

            Gizmos.DrawWireSphere(center, _spawnRadius);

            Gizmos.color = _hit.hit ? Color.blue : Color.red;

            Gizmos.DrawSphere(_hit.hit ? _hit.position : _point, 0.33f);
        }

        protected virtual void QuestStartHandler()
        {
            enabled = true;
        }
    }
}
