using UnityEngine;

namespace MainGame
{
    public class Billboard : MonoBehaviour
    {
        private Camera _camera;
        private Transform _transform;
        
        public void SetCamera(Camera camera)
        {
            _transform = transform;
            _camera = camera;
        }

        private void LateUpdate()
        {
            Vector3 direction = (_camera.transform.position - _transform.position).normalized;
            _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}