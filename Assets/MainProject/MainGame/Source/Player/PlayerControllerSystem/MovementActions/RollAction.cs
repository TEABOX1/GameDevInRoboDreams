using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class RollAction : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CharacterController _characterController;

        private void Start()
        {
            ServiceLocator.Instance.GetService<InputController>().OnRollInput += RollHandler;
        }

        private void RollHandler()
        {
            if(_characterController.isGrounded)
                _playerController.StateMachine.SetState((byte)PlayerControllerState.Roll);
        }
    }
}