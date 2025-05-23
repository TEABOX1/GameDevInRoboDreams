using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class BossFightDetectionArea : MonoBehaviour
    {
        [SerializeField] BossFightArea _bossFightArea;
        [SerializeField] private Dialogue _startFightDialogue;
        [SerializeField] private GameObject _blockWall;

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
                _blockWall.SetActive(true);
                _playerService.Player.TargetPivot.gameObject.SetActive(true);
                Debug.Log($"OnTriggerEnter(Player): _playerService.Player.TargetPivot.gameObject = {_playerService.Player.TargetPivot.gameObject.activeInHierarchy}");
                _bossFightArea.enabled = true;
                _dialogEvents.EnterDialogue(_startFightDialogue);
                gameObject.SetActive(false);
            }
        }
    }
}
