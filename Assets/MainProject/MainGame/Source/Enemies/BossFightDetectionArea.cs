using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class BossFightDetectionArea : MonoBehaviour
    {
        [SerializeField] BossFightArea _bossFightArea;

        private IPlayerService _playerService;
        private QuestEvents _questEvents;
        private DialogueEvents _dialogEvents;

        private void Awake()
        {
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _dialogEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
        }

        private void OnTriggerEnter(Collider colider)
        {
            if (_playerService.IsPlayer(colider))
            {
                _playerService.Player.TargetPivot.gameObject.SetActive(true);
                Debug.Log($"OnTriggerEnter(Player): _playerService.Player.TargetPivot.gameObject = {_playerService.Player.TargetPivot.gameObject.activeInHierarchy}");
                _bossFightArea.enabled = true;
                //TODO: додати виклик діалогу та/або степу квесту
            }
        }
    }
}
