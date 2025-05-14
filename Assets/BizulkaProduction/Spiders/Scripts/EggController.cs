using UnityEngine;

namespace MainGame
{
    public class EggController : MonoBehaviour
    {
        [SerializeField] GameObject _full;
        [SerializeField] GameObject _damaged;
        [SerializeField] private ParticleSystem _particleSystem;

        public void Destroy()
        {
            _full.gameObject.SetActive(false);
            _damaged.gameObject.SetActive(true);
            _particleSystem.Play();
        }
    }
}