using UnityEngine;

namespace RPG.Inventory
{
    public abstract class EquipableItem : InventoryItem, IEquipableItem
    {
        [SerializeField] private EquipLocation equipLocation = EquipLocation.Weapon;
        
        public EquipLocation EquipLocation => equipLocation;
    }
    
}