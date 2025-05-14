using System;
using UnityEngine;

namespace MainGame
{
    public interface IEnemy
    {
        event Action<IEnemy> OnDeath;
        EnemyTypes EnemyType { get; }
        Collider Collider { get; }
    }
}