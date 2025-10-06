using UnityEngine;

namespace RPG.Inventory
{
    public class EquipmentSlot
    {
        public EquipableItem Item { get; private set; }

        public EquipmentSlot()
        {
            Item = null;
        }

        public EquipmentSlot(EquipableItem item)
        {
            Item = item;
        }

        public void SetItem(EquipableItem item)
        {
            Item = item;
        }
        
        public void RemoveItem()
        {
            Item = null;
        }
        
        
    }
}