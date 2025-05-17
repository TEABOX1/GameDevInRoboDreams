using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class CheckpointService : MonoServiceBase
    {
        public event Action OnCheckpointReached;

        public override Type Type { get; } = typeof(CheckpointService);

        [SerializeField] private Collider[] _checkpointColliders;

        private IPlayerService _playerService;

        public Collider[] CheckpointColliders => _checkpointColliders;
        
        private void OnEnable()
        {
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
        }

        public void NotifyTrigger(Collider other)
        {
            if (_playerService.IsPlayer(other))
                OnCheckpointReached?.Invoke();
        }
    }
}
