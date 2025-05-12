using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    //TODO: Remove
    public class EnemyService : MonoServiceBase
    {
        public override Type Type { get; } = typeof(EnemyService);
        
        private readonly List<IEnemy> _enemies = new();
        private readonly Dictionary<Collider, IEnemy> _colliderToEnemyMap = new();

        public event Action<IEnemy> EnemyDied;

        public void RegisterEnemy(IEnemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
                enemy.OnDied += HandleEnemyDied;
            }
            
            foreach (var col in enemy.Colliders)
            {
                _colliderToEnemyMap.TryAdd(col, enemy);
            }
        }

        public void UnregisterEnemy(IEnemy enemy)
        {
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
                enemy.OnDied -= HandleEnemyDied;
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

        public bool TryGetEnemy(Collider collider, out IEnemy enemy)
        {
            return _colliderToEnemyMap.TryGetValue(collider, out enemy);
        }

        public EnemyTypes? GetEnemyType(Collider collider)
        {
            return _colliderToEnemyMap.TryGetValue(collider, out var enemy) ? enemy.EnemyType : null;
        }
        
        private void HandleEnemyDied(IEnemy enemy)
        {
            EnemyDied?.Invoke(enemy);
            UnregisterEnemy(enemy);
        }
    }
}