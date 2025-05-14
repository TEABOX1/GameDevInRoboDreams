using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class QuestTracker : MonoBehaviour
    {
        [SerializeField] private QuestEvents _questEvent;
        [SerializeField] private GameObject _questCanvas;
        [SerializeField] private TextMeshProUGUI _questTask;
        [SerializeField] private QuestPoint _questPoint;

        private void Start()
        {
            _questEvent.OnStartQuest += StartQuestHandler;
            _questEvent.OnAdvanceQuest += AdvanceQuest;
            _questEvent.OnQuestStateChange += QuestStateChangeHandler;
            _questEvent.OnFinishQuest += FinishQuestHandler;

            _questCanvas.SetActive(false);
        }

        private void Update()
        {

        }

        private void StartQuestHandler(string questID)
        {
            _questCanvas.SetActive(true);
            for (int i = 0; i < _questPoint.QuestDialogEntryInfo.Count; i++)
            {
                if (_questPoint.QuestDialogEntryInfo[i].questInfo.QuestId == questID)
                    _questTask.text = _questPoint.QuestDialogEntryInfo[i].questInfo.QuestName;
            }
        }

        private void FinishQuestHandler(string questID)
        {
            _questCanvas.SetActive(false);
        }

        private void AdvanceQuest(string questID)
        {

        }

        private void QuestStateChangeHandler(Quest quest)
        {

        }
    }
}