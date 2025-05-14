using System;
using GlobalSource;

namespace MainGame
{
    public class DialogueEvents : MonoServiceBase
    {
        public override Type Type { get; } = typeof(DialogueEvents);

        public event Action<Dialogue, Action> OnEnterDialogue;
        public event Action<Dialogue> OnCheckDialogue; 
        public event Action<string> OnSelectAnswer;
        public event Action<string> OnLineUpdated;
        public event Action OnExitDialogue;

        public void EnterDialogue(Dialogue dialogue, Action action = null) => OnEnterDialogue?.Invoke(dialogue, action);
        public void CheckDialogue(Dialogue dialogue) => OnCheckDialogue?.Invoke(dialogue);
        public void SelectAnswer(string answer) => OnSelectAnswer?.Invoke(answer);
        public void UpdateLine(string lineId) => OnLineUpdated?.Invoke(lineId);
        public void ExitDialogue() => OnExitDialogue?.Invoke();

        public void ClearAllListeners()
        {
            OnEnterDialogue = null;
            OnCheckDialogue = null;
            OnSelectAnswer = null;
            OnLineUpdated = null;
            OnExitDialogue = null;
        }
    }
}