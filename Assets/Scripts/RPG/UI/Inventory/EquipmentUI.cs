using System;
using System.Collections.Generic;

namespace RPG.UI.Inventory
{
    public class EquipmentUI : PossessionUI
    {
        private void Awake()
        {
            InitializeSlots();
        }

        private void OnEnable()
        {
            RegisterPossessionSlotsDragEvents();
        }

        private void OnDisable()
        {
            UnRegisterPossessionSlotsDragEvents();
        }
        
        protected override void InitializeSlots()
        {
            possessionSlots = new List<PossessionSlotUI>();
            var equipmentSlots = GetComponentsInChildren<EquipmentSlotUI>();
            foreach (var equipmentSlot in equipmentSlots)
            {
                possessionSlots.Add(equipmentSlot);
            }
        }
    }
}