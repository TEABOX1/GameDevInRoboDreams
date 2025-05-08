using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class DynamicHealthSystem : MonoBehaviour
    {
        public event Action<IHealth> OnCharacterDeath;

        private Dictionary<Collider, IHealth> _charactersHealth = new();

        public IHealth this[Collider collider]
        {
            get
            {
                _ = _charactersHealth.TryGetValue(collider, out IHealth health);
                return health;
            }
        }

        public void AddCharacter(IHealth character)
        {
            if (character == null || _charactersHealth.ContainsKey(character.Collider)) return;

            _charactersHealth.Add(character.Collider, character);
            character.OnDeath += () => CharacterDeathHandler(character);
        }

        public void RemoveCharacter(IHealth character)
        {
            if (character == null) return;
            _charactersHealth.Remove(character.Collider);
        }

        public bool GetHealth(Collider characterCollider, out IHealth health) =>
            _charactersHealth.TryGetValue(characterCollider, out health);

        private void CharacterDeathHandler(IHealth health)
        {
            OnCharacterDeath?.Invoke(health);
        }
    }
}
