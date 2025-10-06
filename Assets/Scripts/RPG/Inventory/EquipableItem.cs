using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item")]
    public class EquipableItem : InventoryItem, IEquipableItem
    {
        [SerializeField] private EquipLocation equipLocation = EquipLocation.Weapon;
        
        public EquipLocation EquipLocation => equipLocation;
    }
    
}