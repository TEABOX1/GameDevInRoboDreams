using GlobalSource;
using UnityEngine;

namespace MainGame
{
    /// <summary>
    /// Script that controls footstep sound
    /// Upon receiving animation event, issues into corresponding AudioSource
    /// Request to play and AudioClip, received from IFootstepSoundService
    /// </summary>
    public class FootstepSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _leftFoot;
        [SerializeField] private AudioSource _rightFoot;
        [SerializeField] private GroundDetector _groundDetector;
        
        private IFootstepSoundService _footstepSoundService;
        
        private void Start()
        {
            _footstepSoundService = ServiceLocator.Instance.GetService<IFootstepSoundService>();
        }
        
        public void RightLeg()
        {
            PlayFootstepSound(_rightFoot);
        }
        
        public void LeftLeg()
        {
            PlayFootstepSound(_leftFoot);
        }
        
        private void PlayFootstepSound(AudioSource audioSource)
        {
            // In order to receive AudioClip from footstep system, physics material of a collider, player standing on,
            // Should be received from GroundDetector
            PhysicMaterial key = _groundDetector.Collider.sharedMaterial;
            audioSource.PlayOneShot(_footstepSoundService.GetFootstepSound(key, transform.position));
        }
    }
}