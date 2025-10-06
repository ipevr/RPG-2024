using UnityEngine;

namespace RPG.Inventory
{
    public class InventorySlot
    {
        public InventoryItem Item { get; private set; }
        public int CurrentStackSize { get; private set; }

        public bool IsStackable => Item is IStackableItem;
        public int MaxStackSize => (Item as IStackableItem)?.MaxStackSize ?? 1;

        public InventorySlot()
        {
            Item = null;
            CurrentStackSize = 0;
        }

        public InventorySlot(InventoryItem inventoryItem, int quantity)
        {
            CurrentStackSize = Mathf.Max(1, quantity);
            Item = inventoryItem;
        }

        public void RemoveItem()
        {
            Item = null;
            CurrentStackSize = 0;
        }

        public int GetRemainingSpace(InventoryItem item)
        {
            if (!Item) return GetMaxStackSizeFor(item);
            if (item != Item) return 0;
            
            return MaxStackSize - CurrentStackSize;
        }

        private static int GetMaxStackSizeFor(InventoryItem item)
        {
            return (item as IStackableItem)?.MaxStackSize ?? 1;
        }

        public bool HasFullStack()
        {
            if (!Item) return false;
            if (Item is not IStackableItem) return true;
            
            return CurrentStackSize == MaxStackSize;
        }

        public int AddToStack(int amount)
        {
            if (!Item ||!IsStackable) return 0;
            var canAdd = Mathf.Max(0, MaxStackSize - CurrentStackSize);
            var willAdd = Mathf.Min(amount, canAdd);
            CurrentStackSize += willAdd;
            return willAdd;
        }

        public int RemoveFromStack(int amount)
        {
            var willRemove = Mathf.Min(CurrentStackSize, amount);
            if (willRemove == CurrentStackSize)
            {
                RemoveItem();
            }
            else
            {
                CurrentStackSize -= willRemove;
            }
            return willRemove;
        }
    }
}