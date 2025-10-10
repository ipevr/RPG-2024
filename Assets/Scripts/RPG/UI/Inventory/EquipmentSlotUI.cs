using System;
using UnityEngine;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class EquipmentSlotUI : PossessionSlotUI
    {
        [SerializeField] private EquipLocation location;

        private PlayerEquipment equipment;

        public override void Awake()
        {
            base.Awake();
            equipment = PlayerEquipment.GetPlayerEquipment();
            equipment.onEquipmentChanged.AddListener(RedrawUI);
        }

        private void Start()
        {
            RedrawUI();
        }

        public override InventoryItem GetItem()
        {
            return equipment.GetItem(location);
        }

        public override int GetAmount()
        {
            return equipment.IsEquipped(location) ? 1 : 0;
        }

        public override void RemoveItems(int number)
        {
            equipment.RemoveItem(location);
        }

        public override void AddItems(InventoryItem item, int number)
        {
            equipment.AddItem(item as EquipableItem, location);
        }

        public override int MaxAcceptable(InventoryItem item)
        {
            if (item is not IEquipableItem equipableItem) return 0;
            return equipableItem.EquipLocation != location ? 0 : 1;
        }

        private void RedrawUI()
        {
            icon.SetItem(equipment.GetItem(location));
        }
        
    }
}