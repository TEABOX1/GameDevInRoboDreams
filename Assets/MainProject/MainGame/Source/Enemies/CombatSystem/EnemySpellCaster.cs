using System;
using UnityEngine;

namespace MainGame
{
    public class EnemySpellCaster : MonoBehaviour
    {
        public event Action OnFireballCast; //added for animation

        [SerializeField] private EnemySpellDamageDealer _spellDamageDealer;
        [SerializeField] private EnemySpellData _spellData;
        [SerializeField] private Transform _castPoint;
        //[SerializeField] private NecroSpellCastAnimation _castAnimation;

        Vector3 _attackPoint;

        //added for animation
        [ContextMenu("Force Fireball")]
        private void ForceState()
        {
            Vector3 buf = new Vector3(0,0,0);
            CastSpell();
        }
        //end of animation

        public EnemySpellData EnemySpellData => _spellData;

        public void Awake()
        {
            //_castAnimation.OnFireballAnimationFinished += ProcceedCast;
        }

        public void CastSpell()
        {
            OnFireballCast?.Invoke(); //added for animation
        }

        public void ProcceedCast(Vector3 targetTransform)
        {
            _attackPoint = targetTransform;
            Debug.Log("Necro Cast Spell");
            Vector3 direction = (_attackPoint - _castPoint.position).normalized;
            EnemySpellBase spell = Instantiate(_spellData.SpellPrefab, _castPoint.position, _castPoint.rotation);
            spell.Initialize(direction, _spellData.Speed, _spellDamageDealer);
        }
    }
}
