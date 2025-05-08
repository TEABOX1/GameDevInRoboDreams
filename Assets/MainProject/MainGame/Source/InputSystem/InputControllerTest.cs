using GlobalSource;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class InputControllerTest : MonoBehaviour
    {
        private InputController _inputController;

        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();

            _inputController.OnMovementInput += MovementHandler;
            _inputController.OnLookAroundInput += LookAroundHandler;
            _inputController.OnJumpInput += JumpHandler;
            _inputController.OnRollInput += RollHandler;
            _inputController.OnPrimaryInput += PrimaryHandler;
            _inputController.OnSecondaryInput += SecondaryHandler;
            _inputController.OnInteractInput += InteractHandler;
            _inputController.OnEscapeInput += EscapeHandler;
        }

        private void MovementHandler(Vector2 movementInput, InputDevice device)
        {
            Debug.Log($"[{device}] Movement: {movementInput}");
        }

        private void LookAroundHandler(Vector2 lookAroundInput)
        {
            Debug.Log($"Look Around Input: {lookAroundInput}");
        }

        private void JumpHandler()
        {
            Debug.Log($"Jump Input");
        }

        private void RollHandler()
        {
            Debug.Log($"Roll Input");
        }

        private void PrimaryHandler()
        {
            Debug.Log($"Primary Input");
        }

        private void SecondaryHandler(bool performed)
        {
            Debug.Log($"Secondary Input");
        }

        private void InteractHandler()
        {
            Debug.Log($"Interact Input");
        }

        private void EscapeHandler()
        {
            Debug.Log($"Escape Input");
        }
    }
}