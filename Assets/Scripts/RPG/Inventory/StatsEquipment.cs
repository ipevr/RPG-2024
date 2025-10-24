using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventory
{
    public class StatsEquipment : PlayerEquipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                if (GetItem(slot) is IModifierProvider currentItem)
                {
                    foreach (var modifier in currentItem.GetAdditiveModifiers(stat))
                    {
                        yield return modifier;
                    }
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                if (GetItem(slot) is IModifierProvider currentItem)
                {
                    foreach (var modifier in currentItem.GetPercentageModifiers(stat))
                    {
                        yield return modifier;
                    }
                }
            }
        }
    }
}