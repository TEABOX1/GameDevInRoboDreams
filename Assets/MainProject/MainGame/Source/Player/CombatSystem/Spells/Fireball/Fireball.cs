using UnityEngine;

namespace MainGame.Fireball
{
    public class Fireball : SpellBase
    {
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] ParticleSystem particleSystem;
        [SerializeField] private GameObject _explosionPrefab;
        
        private Vector3 _direction;
        private float _speed;
        private SpellDamageDealer _damageDealer;
        private readonly float _lifeTime = 5f;
        
        public override void Initialize(
            Vector3 direction, 
            float speed, 
            SpellDamageDealer damageDealer)
        {
            // _direction = direction.normalized;
            // _speed = speed;
            _rigidbody.velocity = direction.normalized * speed;
            _damageDealer = damageDealer;
            particleSystem.Play();

            Destroy(gameObject, _lifeTime);
        }

        // private void Update()
        // {
        //     transform.position += _direction * (_speed * Time.deltaTime);
        // }

        private void OnTriggerEnter(Collider collider)
        {
            _damageDealer.DealSpellDamage(transform.position);
            
            particleSystem.Stop();
            _explosionPrefab.SetActive(true);
            
            Destroy(gameObject, 1f);
        }
    }
}