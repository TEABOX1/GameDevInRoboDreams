using System;
using System.Collections;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class DialogueManager : MonoBehaviour
    {
        private Dialogue _currentDialogue;
        private Action _onComplete;
        private bool _dialoguePlaying = false;
        private string _currentLineId;
        private bool _shouldTriggerOnComplete = false;
        
        private DialogueEvents _dialogueEvents;
        private InputController _inputController;
        
        private void OnEnable()
        {
            _dialogueEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
            _dialogueEvents.OnEnterDialogue += EnterDialogueHandler;
            _dialogueEvents.OnSelectAnswer += SelectAnswerHandler;
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
        }

        private void OnDisable()
        {
            _dialogueEvents.OnEnterDialogue -= EnterDialogueHandler;
            _dialogueEvents.OnSelectAnswer -= SelectAnswerHandler;
        }

        private void EnterDialogueHandler(Dialogue dialogue, Action action)
        {
            if(_dialoguePlaying) return;
            
            _dialoguePlaying = true;
            
            if (dialogue.WasPlayed && dialogue.AlternativeDialogue != null)
            {
                _currentDialogue = dialogue.AlternativeDialogue;
            }
            else
            {
                _currentDialogue = dialogue;
                dialogue.WasPlayed = true;
            }
            // _currentDialogue = dialogue;
            _onComplete = action;
            
            _dialogueEvents.CheckDialogue(_currentDialogue);
            
            _inputController.DefaulMapLock();
            _inputController.UIMapLock();
            _inputController.CursorEnable();
            
            ShowLine(_currentDialogue.DialogueData[0].lineId); 
        }
        
        private void ShowLine(string lineId)
        {
            var line = _currentDialogue.GetLineById(lineId);
            if (line == null)
            {
                // Debug.LogWarning("Dialogue line with id " + lineId + " was not found");
                EndDialogue();
                return;
            }

            _currentLineId = lineId;
            
            // Debug.Log($"NPC: {line.Value.dialogueLine}");
            
            _dialogueEvents.UpdateLine(_currentLineId);

            if (line.Value.answerOption == null || line.Value.answerOption.Length <= 0)
            {
                EndDialogue();
            }
            // else
            // {
            //     for (int i = 0; i < line.Value.answerOption.Length; i++)
            //     {
            //         var option = line.Value.answerOption[i];
            //         Debug.Log($"{i + 1}: {option.answerText}");
            //     }
            // }
        }
        
        private void SelectAnswerHandler(string answerText)
        {
            var line = _currentDialogue.GetLineById(_currentLineId);
            if (line == null || line.Value.answerOption == null)
            {
                EndDialogue();
                return;
            }

            // for (int i = 0; i < line.Value.answerOption.Length; i++)
            // {
            //     if (line.Value.answerOption[i].answerText == answerText)
            //     {
            //         ShowLine(line.Value.answerOption[i].nextLineId);
            //         return;
            //     }
            // }

            foreach (var option in line.Value.answerOption)
            {
                if (option.answerText == answerText)
                {
                    if(option.triggerComplete)
                        _shouldTriggerOnComplete = true;
                    ShowLine(option.nextLineId);
                    return;
                }
            }
            
            Debug.LogWarning($"Answer \"{answerText}\" not found.");
        }
        
        private void EndDialogue()
        {
            _dialoguePlaying = false;
            
            _inputController.DefaultMapUnlock();
            _inputController.UIMapUnlock();

            Debug.Log("fuck");
            _dialogueEvents.ExitDialogue();
            
            if(_shouldTriggerOnComplete)
                _onComplete?.Invoke();
            
            _shouldTriggerOnComplete = false;
        }
        
        public void ResetDialoguePlayedState(Dialogue dialogue)
        {
            if (dialogue == null) return;

            dialogue.WasPlayed = false;

            if (dialogue.AlternativeDialogue != null)
                dialogue.AlternativeDialogue.WasPlayed = false;
        }
    }
}