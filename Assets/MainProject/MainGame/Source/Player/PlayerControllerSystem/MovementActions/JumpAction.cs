using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class JumpAction : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CharacterController _characterController;

        private void Start()
        {
            ServiceLocator.Instance.GetService<InputController>().OnJumpInput += JumpHandler;
        }

        private void JumpHandler()
        {
            if (_characterController.isGrounded)
                _playerController.StateMachine.SetState((byte)PlayerControllerState.Jump);
        }
    }
}