using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraYawAnchor;
        [SerializeField] private Transform _cameraPitchAnchor;
        [SerializeField] private float _sensitivity;
        [SerializeField] private float _minPitch = -40f;
        [SerializeField] private float _maxPitch = 70f;
        [SerializeField] private Camera _camera;
        
        public Camera Camera => _camera;
        
        private float _yaw = 0f;
        private float _pitch = 20f;
        
        private void Start()
        {
            ServiceLocator.Instance.GetService<InputController>().OnLookAroundInput += LookAroundHandler;
        }
        
        private void LateUpdate()
        {
            _cameraPitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            _cameraYawAnchor.rotation = Quaternion.Euler(0f, _yaw, 0f);
        }

        private void LookAroundHandler(Vector2 lookInput)
        {
            lookInput *= _sensitivity;

            // _pitch -= lookInput.y;
            
            _pitch -= lookInput.y;
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
            
            _yaw += lookInput.x;
        }
    }
}