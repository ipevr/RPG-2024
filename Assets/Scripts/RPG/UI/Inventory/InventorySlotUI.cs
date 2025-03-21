﻿using RPG.Inventory;
using UnityEngine;
using Utils.UI.Dragging;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon;

        private int index;
        private PlayerInventory inventory;

        public void Setup(PlayerInventory playerInventory, int slotIndex)
        {
            inventory = playerInventory;
            index = slotIndex;
            icon.SetItem(inventory.GetItemInSlot(index));
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
            inventory.AddItemToSlot(index, item);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return 1;
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index);
        }

    }
}