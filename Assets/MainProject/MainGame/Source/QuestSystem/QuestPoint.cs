using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class QuestPoint : InteractableBase
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
        
        protected override void Awake()
        {
            base.Awake();
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
        }

        private void OnDisable()
        {
            _questEvents.OnQuestStateChange -= QuestStateChangeHandler;
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
        public override void Interact()
        {
            // if(!_isPlayerNear) return;
            
            // switch (_currentQuestState)
            // {
            //     case QuestState.CanStart when _startPoint:
            //         _questEvents.StartQuest(_questId);
            //         break;
            //     case QuestState.CanFinish when _finishPoint:
            //         _questEvents.FinishQuest(_questId);
            //         break;
            // }
            
            foreach (var (questId, state) in _questStates)
            {
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
    }
}