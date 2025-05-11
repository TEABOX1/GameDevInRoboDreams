using UnityEngine;

namespace MainGame
{
    public class EnemySpellCaster : MonoBehaviour
    {
        [SerializeField] private EnemySpellDamageDealer _spellDamageDealer;
        [SerializeField] private EnemySpellData _spellData;
        [SerializeField] private Transform _castPoint;

        public EnemySpellData EnemySpellData => _spellData;

        public void CastSpell(Transform targetTransform)
        {
            Vector3 direction = (targetTransform.position - _castPoint.position).normalized;
            EnemySpellBase spell = Instantiate(_spellData.SpellPrefab, _castPoint.position, _castPoint.rotation);
            spell.Initialize(direction, _spellData.Speed, _spellDamageDealer);
        }
    }
}
