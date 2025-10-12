using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Action Item")]
    public class ActionItem : InventoryItem, IStackableItem
    {
        [SerializeField] private bool isConsumable;
        [SerializeField] private int maxStackSize = 5;
        
        public bool IsConsumable => isConsumable;
        public int MaxStackSize =>  maxStackSize;
    }
}