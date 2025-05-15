using System;
using System.Collections.Generic;
using System.Linq;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class EnemyService : MonoServiceBase
    {
        public event Action<IEnemy> EnemyDied;

        public event Action OnBossDefeated;
        public event Action OnAllSpidersDefeated;
        public event Action OnAllSpiderEggsDefeated;

        public override Type Type { get; } = typeof(EnemyService);

        private List<IEnemy> _enemies = new();
        private Dictionary<Collider, IEnemy> _colliderToEnemyMap = new();

        private QuestEvents _questEvents;

        protected override void Awake()
        {
            base.Awake();
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
        }

        public void RegisterEnemy(IEnemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
                Debug.Log($"RegisterEnemy {enemy.Collider.gameObject.name}");
                enemy.OnDeath += HandleEnemyDied;
            }
            
            _colliderToEnemyMap.TryAdd(enemy.Collider, enemy);
        }

        public void UnregisterEnemy(IEnemy enemy)
        {
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
                Debug.Log($"UnRegisterEnemy {enemy.Collider.gameObject.name}");
                enemy.OnDeath -= HandleEnemyDied;
            }
            
            var keysToRemove = new List<Collider>();
            foreach (var pair in _colliderToEnemyMap)
            {
                if (pair.Value == enemy)
                    keysToRemove.Add(pair.Key);
            }

            foreach (var key in keysToRemove)
            {
                _colliderToEnemyMap.Remove(key);
            }
        }

        public bool HasEnemiesOfType(EnemyTypes type) =>
            _colliderToEnemyMap.Values.Any(enemy => enemy.EnemyType == type);

        public bool TryGetEnemy(Collider collider, out IEnemy enemy)
        {
            return _colliderToEnemyMap.TryGetValue(collider, out enemy);
        }

        public EnemyTypes? GetEnemyType(Collider collider)
        {
            return _colliderToEnemyMap.TryGetValue(collider, out var enemy) ? enemy.EnemyType : null;
        }

        public List<Enemy> GetEnemies()
        {
            List<Enemy> _spiders = new();
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].EnemyType != EnemyTypes.Egg)
                    _spiders.Add( (Enemy) _enemies[i]);
            }
            return _spiders;
        }

        public int GetEnemiesOfTypeCount(EnemyTypes type) =>
            _colliderToEnemyMap.Values.Count(enemy => enemy.EnemyType == type);

        private void HandleEnemyDied(IEnemy enemy)
        {
            EnemyDied?.Invoke(enemy);

            if (!HasEnemiesOfType(EnemyTypes.Egg))
            {
                OnAllSpiderEggsDefeated?.Invoke();
                _questEvents.FinishQuest("DestroyEggsQuest");
            }

            if (!HasEnemiesOfType(EnemyTypes.Spider))
            {
                OnAllSpidersDefeated?.Invoke();
                _questEvents.FinishQuest("KillSpidersQuest");
            }

            if (!HasEnemiesOfType(EnemyTypes.Boss))
            {
                OnBossDefeated?.Invoke();
                _questEvents.FinishQuest("QuestInfo");
            }
        }
    }
}