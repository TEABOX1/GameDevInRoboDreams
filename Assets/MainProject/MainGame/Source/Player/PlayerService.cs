using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PlayerService : MonoServiceBase, IPlayerService
    {
        [SerializeField] private TargetableBase _playerTargetable;

        public override Type Type { get; } = typeof(IPlayerService);

        public TargetableBase Player => _playerTargetable;

        private Dictionary<Collider, TargetableBase> _playerTargetables = new();

        protected override void Awake()
        {
            base.Awake();
            _playerTargetables.Add(_playerTargetable.CharacterController, _playerTargetable);
        }

        public bool IsPlayer(Collider collider)
        {
            return _playerTargetables.ContainsKey(collider);
        }
    }
}
