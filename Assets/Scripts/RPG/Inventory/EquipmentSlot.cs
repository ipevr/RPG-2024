using UnityEngine;

namespace RPG.Inventory
{
    public class EquipmentSlot
    {
        public IEquipableItem Item { get; private set; }

        public EquipmentSlot()
        {
            Item = null;
        }

        public EquipmentSlot(IEquipableItem item)
        {
            Item = item;
        }

        public void SetItem(IEquipableItem item)
        {
            Item = item;
        }
        
        public void RemoveItem()
        {
            Item = null;
        }
        
        
    }
}