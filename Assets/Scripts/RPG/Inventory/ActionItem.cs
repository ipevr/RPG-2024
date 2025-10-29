using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Action Item")]
    public class ActionItem : InventoryItem, IStackableItem
    {
        [SerializeField] private bool isConsumable;
        [SerializeField] private int maxStackSize = 5;
        
        public bool IsConsumable => isConsumable;
        public int MaxStackSize => IsStackable ? maxStackSize : 1;

        public virtual void Use(GameObject user)
        {
            Debug.Log($"Using Action Item: {this} by {user.name}");
        }
    }
}