using GlobalSource;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EnemyDetectionArea : MonoBehaviour
    {
        private IPlayerService _playerService;

        private void Awake()
        {
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
        }

        private void OnTriggerEnter(Collider colider)
        {
            if (_playerService.IsPlayer(colider))
            {
                _playerService.Player.TargetPivot.gameObject.SetActive(true);
                Debug.Log($"OnTriggerEnter(Player): _playerService.Player.TargetPivot.gameObject = {_playerService.Player.TargetPivot.gameObject.activeInHierarchy}");
            }
        }

        private void OnTriggerExit(Collider colider)
        {
            List<Enemy> _enemies = new();
            if (_playerService.IsPlayer(colider))
            {
                _playerService.Player.TargetPivot.gameObject.SetActive(false); // можливо потім доведеться вмикати це значення вручну
                Debug.Log($"OnTriggerExit(Player): _playerService.Player.TargetPivot.gameObject = {_playerService.Player.TargetPivot.gameObject.activeInHierarchy}");
            }
        }
    }
}
