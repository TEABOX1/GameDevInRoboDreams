using UnityEngine;

namespace MainGame
{
    public class MapMarker : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private Transform _transform;

        public Color Color => _color;
        public Transform Transform => _transform;
    }
}
