using UnityEngine;

namespace MainGame
{
    public abstract class EnemySpellBase : MonoBehaviour
    {
        public abstract void Initialize(
            Vector3 direction,
            float speed,
            EnemySpellDamageDealer damageDealer);
    }
}
