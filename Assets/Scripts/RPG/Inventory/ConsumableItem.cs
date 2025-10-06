using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Consumable Item")]
    public class ConsumableItem : InventoryItem, IStackableItem
    {
        [Tooltip("If 1, item can not be stacked. Otherwise gives the number of how much items of this type can be stacked in the same inventory slot.")]
        [Range(1, 100)]
        [SerializeField] private int maxStackSize = 1;

        public int MaxStackSize => maxStackSize;
    }
}