using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class DruidAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private QuestPoint _questPoint;
        [SerializeField] private float _crossFadeTime = 0.5f;
        
        private DialogueEvents _dialogueEvents;
        
        [Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _talkName;

        private int _idleId;
        private int _talkId;

        private void Awake()
        {
            _idleId = Animator.StringToHash(_idleName);
            _talkId = Animator.StringToHash(_talkName);

            _dialogueEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
        }

        private void OnEnable()
        {
            _questPoint.OnInteract += InteractHandler;
            _dialogueEvents.OnExitDialogue += ExitDialogueHandler;
        }

        private void OnDisable()
        {
            _questPoint.OnInteract -= InteractHandler;
            _dialogueEvents.OnExitDialogue -= ExitDialogueHandler;
        }

        private void InteractHandler()
        {
            Debug.Log("Interact");
            _animator.CrossFadeInFixedTime(_talkId, _crossFadeTime);
        }

        private void ExitDialogueHandler()
        {
            Debug.Log("Exit Dialogue");
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
        }
    }
}