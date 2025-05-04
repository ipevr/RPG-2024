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
            dragItem.onBeginDrag.AddListener(HandleBeginDrag);
            dragItem.onEndDrag.AddListener(HandleEndDrag);
        }

        private void OnDisable()
        {
            dragItem.onDragging.RemoveListener(HandleDragging);
            dragItem.onBeginDrag.RemoveListener(HandleBeginDrag);
            dragItem.onEndDrag.RemoveListener(HandleEndDrag);
        }

        public void Setup(PlayerInventory playerInventory, int slotIndex)
        {
            inventory = playerInventory;
            index = slotIndex;
            icon.SetItem(inventory.GetItemInSlot(index));
            amountDisplay.SetAmount(inventory.GetAmountInSlot(index));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return inventory.MaxAcceptable(item);
        }

        public void AddItems(InventoryItem item, int number)
        {
            inventory.AddItemsBeginningAtSlot(index, item, number);
        }

        public int GetAmount()
        {
            return inventory.GetAmountInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }
        
        private void HandleBeginDrag()
        {
            amountDisplay.SetAmount(0);
        }

        private void HandleEndDrag()
        {
            amountDisplay.SetAmount(inventory.GetAmountInSlot(index));
        }

        private void HandleDragging(bool dragging)
        {
            // if (dragging)
            // {
            //     if (inventory.GetNumberInSlot(index) > 1)
            //     {
            //         tempIcon = Instantiate(icon.gameObject, icon.transform.parent);
            //         amountDisplay.SetAmount(inventory.GetNumberInSlot(index) - 1);
            //     }
            // }
            // else
            // {
            //     if (tempIcon)
            //     {
            //         Destroy(tempIcon);
            //         amountDisplay.SetAmount(inventory.GetNumberInSlot(index));
            //     }
            // }
        }
    }
}