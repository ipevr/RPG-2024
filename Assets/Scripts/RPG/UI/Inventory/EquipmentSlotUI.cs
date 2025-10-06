using System;
using RPG.Inventory;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class EquipmentSlotUI : PossessionSlotUI
    {
        [SerializeField] private EquipLocation location;
        
        private PlayerEquipment equipment;

        private void Awake()
        {
            equipment = PlayerEquipment.GetPlayerEquipment();
        }

        public void Setup(PlayerEquipment playerEquipment)
        {
            equipment = playerEquipment;
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
            return 1;
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