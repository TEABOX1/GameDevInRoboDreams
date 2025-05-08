using UnityEngine;

namespace MainGame
{
    public abstract class SpellBase : MonoBehaviour
    {
        public abstract void Initialize(
            Vector3 direction, 
            float speed,
            SpellDamageDealer damageDealer);
    }
}