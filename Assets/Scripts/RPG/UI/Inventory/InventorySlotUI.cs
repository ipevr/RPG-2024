using System;
using UnityEngine;
using Utils.UI.Dragging;
using RPG.Inventory;
using UnityEngine.InputSystem;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon;
        [SerializeField] private InventoryDragItem dragItem;
        [SerializeField] private AmountDisplay amountDisplay;
        [SerializeField] private InputAction stackMoveAction;
        
        public InventoryDragItem DragItem => dragItem;

        private int index;
        private PlayerInventory inventory;
        private GameObject tempIcon;

        private void OnEnable()
        {
            dragItem.onDragging.AddListener(HandleDragging);
            stackMoveAction.Enable();
        }

        private void OnDisable()
        {
            dragItem.onDragging.RemoveListener(HandleDragging);
            stackMoveAction.Disable();
        }

        public void Setup(PlayerInventory playerInventory, int slotIndex)
        {
            inventory = playerInventory;
            index = slotIndex;
            icon.SetItem(inventory.GetItemInSlot(index));
            amountDisplay.SetAmount(inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return inventory.MaxAcceptable(item);
        }

        public void AddItems(InventoryItem item, int number)
        {
            inventory.AddItemsBeginningAtSlot(index, item, number);
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
        
        private void HandleDragging(bool dragging)
        {
            if (dragging)
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