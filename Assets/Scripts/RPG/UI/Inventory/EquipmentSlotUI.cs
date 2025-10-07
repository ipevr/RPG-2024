using System;
using UnityEngine;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class EquipmentSlotUI : PossessionSlotUI
    {
        [SerializeField] private EquipLocation location;

        public EquipLocation Location => location;

        private PlayerEquipment equipment;

        public void Setup(PlayerEquipment playerEquipment)
        {
            equipment = playerEquipment;
        }

        public void SetItem(EquipableItem item)
        {
            equipment.PutInSlot(item, location);
            icon.SetItem(equipment.GetItemInSlot(location));
        }

        public override InventoryItem GetItem()
        {
            return equipment.GetItemInSlot(location);
        }

        public override int GetAmount()
        {
            return equipment.IsEquipped(location) ? 1 : 0;
        }

        public override void RemoveItems(int number)
        {
            equipment.RemoveFromSlot(location);
            icon.SetItem(null);
        }

        public override void AddItems(InventoryItem item, int number)
        {
            equipment.PutInSlot(item as EquipableItem, location);
            icon.SetItem(equipment.GetItemInSlot(location));
        }

        public override int MaxAcceptable(InventoryItem item)
        {
            if (item is not IEquipableItem equipableItem) return 0;
            
            return equipableItem.EquipLocation != location ? 0 : 1;
        }
        
        protected override void HandleBeginDrag(bool singleMode)
        {
            
        }

        protected override void HandleEndDrag(bool singleMode)
        {
            
        }

        protected override void HandleDragging(bool dragging)
        {
            
        }
    }
}