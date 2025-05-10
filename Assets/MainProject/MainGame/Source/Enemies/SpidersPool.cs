using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainGame
{
    public class SpidersPool : MonoBehaviour
    {
        [SerializeField] private SpiderEnemyController _enemyPrefab;
        [SerializeField] private int _initialPoolSize = 3;
        [SerializeField] private int _maxPoolSize = 12;
        [SerializeField] private bool _autoExpand = true; //можна встановити обмеження на кількість павуків

        private List<SpiderEnemyController> _pool = new List<SpiderEnemyController>();

        public int ActiveCount => _pool.Count(e => e.gameObject.activeInHierarchy);
        public int TotalCount => _pool.Count;

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateEnemy(false);
            }
        }

        private SpiderEnemyController CreateEnemy(bool activateImmediately)
        {
            var enemy = Instantiate(_enemyPrefab, transform);
            enemy.gameObject.SetActive(activateImmediately);
            _pool.Add(enemy);
            return enemy;
        }

        public SpiderEnemyController GetEnemy(Vector3 position, Quaternion rotation)
        {
            foreach (var enemy in _pool)
            {
                if (!enemy.gameObject.activeInHierarchy)
                {
                    PrepareEnemy(enemy, position, rotation);
                    return enemy;
                }
            }

            if (_autoExpand && _pool.Count < _maxPoolSize)
            {
                var newEnemy = CreateEnemy(true);
                PrepareEnemy(newEnemy, position, rotation);
                return newEnemy;
            }

            Debug.LogWarning("No available enemies in pool");
            return null;
        }

        private void PrepareEnemy(SpiderEnemyController enemy, Vector3 position, Quaternion rotation)
        {
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.gameObject.SetActive(true);
            enemy.ResetEnemy();
        }

        public void ReturnEnemy(SpiderEnemyController enemy)
        {
            if (enemy == null) return;

            enemy.gameObject.SetActive(false);
            enemy.transform.SetParent(transform);
        }

        public void ReturnAllEnemies()
        {
            foreach (var enemy in _pool)
            {
                if (enemy.gameObject.activeInHierarchy)
                {
                    ReturnEnemy(enemy);
                }
            }
        }
    }
}