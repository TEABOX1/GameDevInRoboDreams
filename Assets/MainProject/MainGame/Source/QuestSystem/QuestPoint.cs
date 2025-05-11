using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class QuestPoint : MonoBehaviour
    {
        // [SerializeField] private QuestInfo _questInfo;
        [SerializeField] private List<QuestInfo> _questInfo;
        [SerializeField] private QuestIcon _questIcon;
        
        [Header("Config")]
        [SerializeField] private bool _startPoint = true;
        [SerializeField] private bool _finishPoint = true;
        
        // private string _questId;
        // private QuestState _currentQuestState;
        private Dictionary<string, QuestState> _questStates = new();
        
        private QuestEvents _questEvents;
        private InputController _inputController;
        
        private void Awake()
        {
            // _questId = _questInfo.QuestId;
            foreach (var info in _questInfo)
            {
                _questStates[info.QuestId] = QuestState.RequirementNotMet;
            }
        }

        private void OnEnable()
        {
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _questEvents.OnQuestStateChange += QuestStateChangeHandler;
            //TODO: Remove
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnInteractInput += InteractInputHandler;
        }

        private void OnDisable()
        {
            _questEvents.OnQuestStateChange -= QuestStateChangeHandler;
            //TODO: Remove
            _inputController.OnInteractInput -= InteractInputHandler;
        }
        
        private void QuestStateChangeHandler(Quest quest)
        {
            // if (quest.QuestInfo.QuestId.Equals(_questId))
            // {
            //     _currentQuestState = quest.QuestState;
            //     // Debug.Log($"Quest with id: {_questId}, state: {_currentQuestState}");
            //     _questIcon.SetState(_currentQuestState, _startPoint, _finishPoint);
            // }
            
            string questId = quest.QuestInfo.QuestId;
            if (_questStates.ContainsKey(questId))
            {
                _questStates[questId] = quest.QuestState;
                _questIcon.SetState(quest.QuestState, _startPoint, _finishPoint);
            }
        }
        //TODO: Remove all and change for interactable system
        private bool _isPlayerNear = false;
        private void InteractInputHandler()
        {
            if(!_isPlayerNear) return;
            
            // switch (_currentQuestState)
            // {
            //     case QuestState.CanStart when _startPoint:
            //         _questEvents.StartQuest(_questId);
            //         break;
            //     case QuestState.CanFinish when _finishPoint:
            //         _questEvents.FinishQuest(_questId);
            //         break;
            // }
            
            foreach (var kvp in _questStates)
            {
                string questId = kvp.Key;
                QuestState state = kvp.Value;

                switch (state)
                {
                    case QuestState.CanStart when _startPoint:
                        _questEvents.StartQuest(questId);
                        return;
                    case QuestState.CanFinish when _finishPoint:
                        _questEvents.FinishQuest(questId);
                        return;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
                _isPlayerNear = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
                _isPlayerNear = false;
        }
    }
}