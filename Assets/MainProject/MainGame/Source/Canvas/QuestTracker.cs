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

        [SerializeField] private GameObject _needToDoMark;
        [SerializeField] private GameObject _doneMark;

        private void OnEnable()
        {
            _questEvent.OnStartQuest += StartQuestHandler;
            _questEvent.OnAdvanceQuest += AdvanceQuest;
            _questEvent.OnQuestStateChange += QuestStateChangeHandler;
            _questEvent.OnFinishQuest += FinishQuestHandler;
            _questEvent.OnQuestStepStateChange += QuestStepStateChange;

            _needToDoMark.SetActive(false);
            _doneMark.SetActive(false);
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
            if (quest.QuestState != QuestState.InProgress && quest.QuestState != QuestState.CanFinish)
                return;

            if (quest.QuestState == QuestState.InProgress)
            {
                _needToDoMark.SetActive(true);
                _doneMark.SetActive(false);
            }
            if (quest.QuestState == QuestState.CanFinish)
            {
                _needToDoMark.SetActive(false);
                _doneMark.SetActive(true);
            }

                _questCanvas.SetActive(true);
            for (int i = 0; i < _questPoint.QuestDialogEntryInfo.Count; i++)
            {
                if (_questPoint.QuestDialogEntryInfo[i].questInfo.QuestId == quest.QuestInfo.QuestId)
                    _questTask.text = _questPoint.QuestDialogEntryInfo[i].questInfo.QuestName;
            }
        }

        private void QuestStepStateChange(string questID, int stepIndex, QuestStepState questStepState)
        {
            //if (questStepState.QuestState != QuestState.InProgress || quest.QuestState != QuestState.CanFinish)
            //    return;
            _questCanvas.SetActive(true);
            for (int i = 0; i < _questPoint.QuestDialogEntryInfo.Count; i++)
            {
                if (_questPoint.QuestDialogEntryInfo[i].questInfo.QuestId == questID)
                    _questTask.text = _questPoint.QuestDialogEntryInfo[i].questInfo.QuestName;
            }
        }
    }
}