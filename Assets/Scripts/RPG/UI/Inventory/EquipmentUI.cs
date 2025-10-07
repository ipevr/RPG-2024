using System;
using System.Collections.Generic;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class EquipmentUI : PossessionUI
    {        
        private PlayerEquipment playerEquipment;


        private void Awake()
        {
            playerEquipment = PlayerEquipment.GetPlayerEquipment();
        }

        private void Start()
        {
            InitializeSlots();
            RegisterPossessionSlotsDragEvents();
            playerEquipment.OnEquipmentRestored.AddListener(InitializeSlots);
        }

        private void OnDisable()
        {
            UnRegisterPossessionSlotsDragEvents();
            playerEquipment.OnEquipmentRestored.RemoveListener(InitializeSlots);
        }
        
        protected override void InitializeSlots()
        {
            slots = new List<PossessionSlotUI>();
            var equipmentSlots = GetComponentsInChildren<EquipmentSlotUI>();
            foreach (var equipmentSlot in equipmentSlots)
            {
                equipmentSlot.Setup(playerEquipment);
                equipmentSlot.SetItem(playerEquipment.GetItemInSlot(equipmentSlot.Location));
                slots.Add(equipmentSlot);
            }
        }
    }
}
