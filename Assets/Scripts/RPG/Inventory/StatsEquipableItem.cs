using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item")]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] private Modifier[] additiveModifiers;
        [SerializeField] private Modifier[] percentageModifiers;

        [System.Serializable]
        public struct Modifier
        {
            public Stat stat;
            public float value;
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            return GetModifiersValue(additiveModifiers, stat);
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            return GetModifiersValue(percentageModifiers, stat);
        }

        private static IEnumerable<float> GetModifiersValue(Modifier[] modifiers, Stat stat)
        {
            foreach (var modifier in modifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}