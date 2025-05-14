using UnityEngine;

namespace MainGame
{
    public class NecroFireball : EnemySpellBase
    {
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] ParticleSystem particleSystem;

        private EnemySpellDamageDealer _damageDealer;

        public override void Initialize(Vector3 direction, float speed, EnemySpellDamageDealer damageDealer)
        {
            _rigidbody.velocity = direction.normalized * speed;
            _damageDealer = damageDealer;

            particleSystem.Play();

            Destroy(gameObject, _lifeTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _damageDealer.DealSpellDamage(transform.position);

            Destroy(gameObject);
        }
    }
}
