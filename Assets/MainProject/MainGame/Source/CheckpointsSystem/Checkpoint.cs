using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class Checkpoint : MonoBehaviour
    {
        private CheckpointService _checkpointService;

        private void OnEnable()
        {
            _checkpointService = ServiceLocator.Instance.GetService<CheckpointService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _checkpointService.NotifyTrigger(other);
        }
    }
}