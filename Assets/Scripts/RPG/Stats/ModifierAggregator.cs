using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public static class ModifierAggregator
    {
        public static float GetAdditive(GameObject character, Stat stat)
        {
            var total = 0f;
            foreach (var provider in GetProviders(character))
            {
                foreach (var modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        public static float GetPercentage(GameObject character, Stat stat)
        {
            var total = 0f;
            foreach (var provider in GetProviders(character))
            {
                foreach (var modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        public static float GetPercentageFraction(GameObject character, Stat stat)
        {
            var additionalPercentage = GetPercentage(character, stat);
            return 1 + additionalPercentage / 100f;
        }

        private static IEnumerable<IModifierProvider> GetProviders(GameObject character)
        {
            return character.GetComponents<IModifierProvider>();
        }
    }
}