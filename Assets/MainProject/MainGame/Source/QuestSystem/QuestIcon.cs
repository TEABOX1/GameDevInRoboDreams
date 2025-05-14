using System;
using UnityEngine;

namespace MainGame
{

    public class QuestIcon : MonoBehaviour
    {
        [Header("Icons")]
        [SerializeField] private GameObject _requirementsNotMetToStartIcon;
        [SerializeField] private GameObject _canStartIcon;
        [SerializeField] private GameObject _requirementsNotMetToFinishIcon;
        [SerializeField] private GameObject _canFinishIcon;

        public void OnEnable()
        {
            SetState(QuestState.RequirementNotMet, true, true);
        }

        public void SetState(QuestState newState, bool startPoint, bool finishPoint)
        {
            _requirementsNotMetToStartIcon.SetActive(false);
            _canStartIcon.SetActive(false);
            _requirementsNotMetToFinishIcon.SetActive(false);
            _canFinishIcon.SetActive(false);
            
            switch (newState)
            {
                case QuestState.RequirementNotMet:
                    if (startPoint) _requirementsNotMetToStartIcon.SetActive(true);
                    break;
                case QuestState.CanStart:
                    if (startPoint) _canStartIcon.SetActive(true);
                    break;
                case QuestState.InProgress:
                    if (finishPoint) _requirementsNotMetToFinishIcon.SetActive(true);
                    break;
                case QuestState.CanFinish:
                    if (finishPoint) _canFinishIcon.SetActive(true);
                    break;
                case QuestState.Finished:
                    return;
                default:
                    Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                    break;
            }
        }
    }
}