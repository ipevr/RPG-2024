using RPG.Inventory;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : PossessionSlotUI
    {
        [SerializeField] private AmountDisplay amountDisplay;
        
        private PlayerInventory inventory;
        private int index;

        public void Setup(PlayerInventory playerInventory, int slotIndex)
        {
            inventory = playerInventory;
            index = slotIndex;
            icon.SetItem(GetItem());
            amountDisplay.SetAmount(GetAmount());
        }
        
        public override int MaxAcceptable(InventoryItem item)
        {
            return inventory.MaxAcceptable(item);
        }

        public override void AddItems(InventoryItem item, int number)
        {
            inventory.AddItemsBeginningAtSlot(index, item, number);
        }

        public override int GetAmount()
        {
            // Todo: Implement Single-Drag of items. To implement it properly, the dragging util has to be modified.
            // In dragging util, a stack where only one item is dragged, must remain visible.
            // if (Keyboard.current.shiftKey.isPressed && inventory.GetAmountInSlot(index) > 1)
            // {
            //     return 1;
            // }
            return inventory.GetAmountInSlot(index);
        }

        public override void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }

        public override InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        protected override void HandleBeginDrag(bool singleMode)
        {
            if (singleMode)
            {
                amountDisplay.SetAmount(inventory.GetAmountInSlot(index) - 1);
                // inventory.RemoveFromSlot(index, 1);
            }
            else
            {
                amountDisplay.SetAmount(0);
            }
        }

        protected override void HandleEndDrag(bool singleMode)
        {
            amountDisplay.SetAmount(inventory.GetAmountInSlot(index));
        }
    }
}