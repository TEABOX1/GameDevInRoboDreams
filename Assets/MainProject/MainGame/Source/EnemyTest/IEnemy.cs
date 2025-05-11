using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    //TODO: Remove
    public interface IEnemy
    {
        event Action<IEnemy> OnDied;
        EnemyTypes EnemyType { get; }
        IReadOnlyList<Collider> Colliders { get; }
    }
}