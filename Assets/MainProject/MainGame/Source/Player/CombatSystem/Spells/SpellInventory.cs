using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class SpellInventory : MonoServiceBase
    {
        public event Action<SpellData> OnSpellUnlocked;
        
        public override Type Type => typeof(SpellInventory);

        private readonly List<SpellData> _unlockedSpells = new();

        public IReadOnlyList<SpellData> UnlockedSpells => _unlockedSpells;

        public void UnlockSpell(SpellData spellData)
        {
            if (_unlockedSpells.Contains(spellData)) return;
            
            _unlockedSpells.Add(spellData);
            OnSpellUnlocked?.Invoke(spellData);
            Debug.Log($"Spell unlocked: {spellData.name}");
        }

        public bool HasSpell(SpellData spellData) => _unlockedSpells.Contains(spellData);
    }
}