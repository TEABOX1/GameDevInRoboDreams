using GlobalSource;
using MainGame;
using UnityEngine;

namespace MainProject.MainGame.Source.Player
{
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] private Health _health;
        
        private IHealthService _healthService;
        
        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _healthService.AddCharacter(_health);

            _health.OnHealthChanged += HealthChangedHandler;
            _health.OnDeath += DeathHandler;
        }

        private void HealthChangedHandler(int health)
        {
            Debug.Log($"{gameObject.name} health changed to {health}");
        }

        private void DeathHandler()
        {
            Debug.Log($"{gameObject.name} died");
            Destroy(gameObject);
        }
    }
}