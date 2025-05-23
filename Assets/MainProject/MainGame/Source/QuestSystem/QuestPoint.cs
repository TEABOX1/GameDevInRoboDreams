using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEditor;
using UnityEngine;

namespace MainGame
{
    public class QuestPoint : InteractableBase
    {
        public event Action OnInteract;
        
        [Serializable]
        public class QuestDialogueEntry
        {
            public QuestInfo questInfo;
            [Header("Dialogues")]
            public Dialogue startDialogue;
            public Dialogue inProgressDialogue;
            public Dialogue finishDialogue;
            [Header("Transform")]
            public Transform newNPCPosition;
            [Header("Location lock")]
            public GameObject lockLocationObject;
        }
        
        [SerializeField] private List<QuestDialogueEntry> _questDialogues;
        
        // [SerializeField] private QuestInfo _questInfo;
        // [SerializeField] private List<QuestInfo> _questInfo;
        [SerializeField] private QuestIcon _questIcon;
        
        [Header("Config")]
        [SerializeField] private bool _startPoint = true;
        [SerializeField] private bool _finishPoint = true;
        
        [Header("Dialogues")]
        [SerializeField] private Dialogue _requirementNotMetDialogue;
        
        // private string _questId;
        // private QuestState _currentQuestState;
        private Dictionary<string, QuestState> _questStates = new();
        
        private QuestEvents _questEvents;
        private DialogueEvents _dialogueEvents;
        private InputController _inputController;
        
        public List<QuestDialogueEntry> QuestDialogEntryInfo { get { return _questDialogues; } }
        
        protected override void Awake()
        {
            base.Awake();
            // _questId = _questInfo.QuestId;
            foreach (var info in _questDialogues)
            {
                string questId = info.questInfo.QuestId;
                _questStates[questId] = QuestState.RequirementNotMet;
                if(info.lockLocationObject)
                    info.lockLocationObject.SetActive(false);
            }
            
            // UpdateQuestIcon();
        }

        private void OnEnable()
        {
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _questEvents.OnQuestStateChange += QuestStateChangeHandler;
            
            _dialogueEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
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
            // Debug.Log($"[QuestPoint] State change: {questId} -> {quest.QuestState}");
            if (_questStates.ContainsKey(questId))
            {
                _questStates[questId] = quest.QuestState;
                // _questIcon.SetState(quest.QuestState, _startPoint, _finishPoint);
            }
            UpdateQuestNPC();
        }
        
        private void UpdateQuestNPC()
        {
            for (int i = 0; i < _questDialogues.Count; i++)
            {
                string questId = _questDialogues[i].questInfo.QuestId;
                if (!_questStates.TryGetValue(questId, out var state))
                    continue;

                if (state == QuestState.Finished)
                {
                    if (_questDialogues[i].lockLocationObject)
                        _questDialogues[i].lockLocationObject.SetActive(true);
                    continue;
                }

                if(state == QuestState.CanStart)
                {
                    if(i != 0)
                    {
                        transform.SetPositionAndRotation(_questDialogues[i-1].newNPCPosition.position,
                       _questDialogues[i-1].newNPCPosition.rotation);
                    }
                }

                if(state == QuestState.CanFinish)
                    transform.SetPositionAndRotation(_questDialogues[i].newNPCPosition.position,
                        _questDialogues[i].newNPCPosition.rotation);
                
                _questIcon.SetState(state, _startPoint, _finishPoint);
                return;
            }
            
            _questIcon.SetState(QuestState.Finished, _startPoint, _finishPoint);
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
            
            // foreach (var (questId, state) in _questStates)
            // {
            //     switch (state)
            //     {
            //         case QuestState.CanStart when _startPoint:
            //             _questEvents.StartQuest(questId);
            //             return;
            //         case QuestState.CanFinish when _finishPoint:
            //             _questEvents.FinishQuest(questId);
            //             return;
            //     }
            // }
            
            foreach (var entry in _questDialogues)
            {
                string questId = entry.questInfo.QuestId;
                if (!_questStates.TryGetValue(questId, out var state))
                    continue;
                
                switch (state)
                {
                    case QuestState.CanFinish when _finishPoint:
                        TriggerDialogue(entry.finishDialogue, () =>
                        {
                            _questEvents.FinishQuest(questId);
                        });
                        return;
                    case QuestState.CanStart when _startPoint:
                        TriggerDialogue(entry.startDialogue, () =>
                        {
                            _questEvents.StartQuest(questId);
                        });
                        return;
                    case QuestState.InProgress when _startPoint || _finishPoint:
                        TriggerDialogue(entry.inProgressDialogue);
                        return;
                    case QuestState.RequirementNotMet when _startPoint:
                        TriggerDialogue(_requirementNotMetDialogue);
                        return;
                }
            }
        }

        private void TriggerDialogue(Dialogue dialogue, Action onComplete = null)
        {
            if (!dialogue)
            {
                onComplete?.Invoke();
                return;
            }
            OnInteract?.Invoke();
            _dialogueEvents.EnterDialogue(dialogue, onComplete);
            // onComplete?.Invoke();
        }
    }
}