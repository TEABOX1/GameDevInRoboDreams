using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class MinimapController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private Vector2 _worldMin;
        [SerializeField] private Vector2 _worldMax;
        [SerializeField] private Vector2 _mapMin;
        [SerializeField] private Vector2 _mapMax;
        [SerializeField] private Vector2 _tiling = Vector2.one;
        [SerializeField] private MapMarker[] _markers;
        [SerializeField] private Image _markerPrefab;
        [SerializeField] private Transform _content;

        private Image[] _minimapMarkers;

        private void Start()
        {
            _minimapMarkers = new Image[_markers.Length];

            for (int i = 0; i < _markers.Length; ++i)
            {
                Image marker = Instantiate(_markerPrefab, _content);
                marker.gameObject.SetActive(true);
                _minimapMarkers[i] = marker;
            }
        }

        private void Update()
        {
            SetWorldPositionOnMap(_playerTransform.position);

            for (int i = 0; i < _markers.Length; ++i)
            {
                Image markerImage = _minimapMarkers[i];
                MapMarker mapMarker = _markers[i];
                //markerImage.rectTransform.anchoredPosition = GetWorldPositionOnMap(mapMarker.Transform.position);
                markerImage.rectTransform.anchoredPosition = ClampPositionToContent(GetWorldPositionOnMap(mapMarker.Transform.position));
                markerImage.color = mapMarker.Color;
            }
        }

        public void SetWorldPositionOnMap(Vector3 worldPos)
        {
            float normX = Mathf.InverseLerp(_worldMin.x, _worldMax.x, worldPos.x);
            float normY = Mathf.InverseLerp(_worldMin.y, _worldMax.y, worldPos.z);

            Vector2 uv = new Vector2(normX, normY);
            Vector2 offset = uv - _tiling * 0.5f;

            _rawImage.uvRect = new Rect(offset, _tiling);
        }

        public Vector2 GetWorldPositionOnMap(Vector3 worldPos)
        {
            worldPos = worldPos - _playerTransform.position;
            Vector2 worldSize = _worldMax - _worldMin;

            float normX = Mathf.InverseLerp(-worldSize.x, worldSize.x, worldPos.x);
            float normY = Mathf.InverseLerp(-worldSize.y, worldSize.y, worldPos.z);

            Vector2 position;
            position.x = Mathf.Lerp(_mapMin.x, _mapMax.x, normX);
            position.y = Mathf.Lerp(_mapMin.y, _mapMax.y, normY);

            return position;
        }

        private Vector2 ClampPositionToContent(Vector2 position)
        {
            if (_content == null) return position;

            RectTransform contentRect = _content as RectTransform;
            Vector2 contentSize = contentRect.rect.size * 0.5f;

            position.x = Mathf.Clamp(position.x, -contentSize.x, contentSize.x);
            position.y = Mathf.Clamp(position.y, -contentSize.y, contentSize.y);

            return position;
        }
    }
}
