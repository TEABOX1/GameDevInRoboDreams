using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GlobalSource
{
    public class HoverButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private AudioSource _interractSource;
        [SerializeField] private AudioClip _hoveredAudioClip;
        [SerializeField] private AudioClip _pressedAudioClip;

        private bool isHovering = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            PlayHoverSound();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //PlayPressedSound();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PlayPressedSound();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayPressedSound();
        }

        private void PlayHoverSound()
        {
            if (_hoveredAudioClip != null)
            {
                _interractSource.clip = _hoveredAudioClip;
                _interractSource.Play();
            }
        }

        private void PlayPressedSound()
        {
            if (_pressedAudioClip != null)
            {
                _interractSource.clip = _pressedAudioClip;
                _interractSource.Play();
            }
        }
    }
}