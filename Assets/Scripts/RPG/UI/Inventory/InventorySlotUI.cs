using System;
using UnityEngine;
using Utils.UI.Dragging;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon;
        [SerializeField] private InventoryDragItem dragItem;
        [SerializeField] private AmountDisplay amountDisplay;

        public InventoryDragItem DragItem => dragItem;

        private int index;
        private PlayerInventory inventory;
        private GameObject tempIcon;

        private void OnEnable()
        {
            dragItem.onDragging.AddListener(HandleDragging);
        }

        private void OnDisable()
        {
            dragItem.onDragging.RemoveListener(HandleDragging);
        }

        public void Setup(PlayerInventory playerInventory, int slotIndex)
        {
            inventory = playerInventory;
            index = slotIndex;
            icon.SetItem(inventory.GetItemInSlot(index));
            amountDisplay.SetAmount(inventory.GetNumberInSlot(index));
        }

        // Will be detailed when using stackable items. At the moment it says put in as much as you want if there is
        // still nothing, otherwise if anything is already in, nothing more is accepted.
        public int MaxAcceptable(InventoryItem item)
        {
            return inventory.HasSpaceFor(item) ? int.MaxValue : 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            // number will be used later for stackable items
            inventory.AddItemsToSlot(index, item, number);
        }

        public int GetNumber()
        {
            return 1;
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }
        
        private void HandleDragging(bool status)
        {
            if (status)
            {
                if (inventory.GetNumberInSlot(index) > 1)
                {
                    tempIcon = Instantiate(icon.gameObject, icon.transform.parent);
                    amountDisplay.SetAmount(inventory.GetNumberInSlot(index) - 1);
                }
            }
            else
            {
                if (tempIcon)
                {
                    Destroy(tempIcon);
                    amountDisplay.SetAmount(inventory.GetNumberInSlot(index));
                }
            }
        }
    }
}