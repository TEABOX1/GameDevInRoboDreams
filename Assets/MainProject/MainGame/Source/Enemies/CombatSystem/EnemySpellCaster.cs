using UnityEngine;

namespace MainGame
{
    public class EnemySpellCaster : MonoBehaviour
    {
        [SerializeField] private SpellDamageDealer _spellDamageDealer;
        [SerializeField] private SpellData _spellData;
        [SerializeField] private Transform _castPoint;

        public SpellData SpellData => _spellData;

        public void CastSpell(Transform targetTransform)
        {
            Vector3 direction = (targetTransform.position - _castPoint.position).normalized;
            SpellBase spell = Instantiate(_spellData.SpellPrefab, _castPoint.position, _castPoint.rotation);
            spell.Initialize(direction, _spellData.Speed, _spellDamageDealer);
        }
    }
}
