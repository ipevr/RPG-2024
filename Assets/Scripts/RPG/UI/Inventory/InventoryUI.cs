using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class InventoryUI : PossessionUI
    {
        [SerializeField] private InventorySlotUI inventorySlotPrefab;

        private PlayerInventory playerInventory;

        private void Awake()
        {
            playerInventory = PlayerInventory.GetPlayerInventory();
            playerInventory.OnInventoryChanged += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            DestroyAllSlots();
            InitializeSlots();
            RegisterPossessionSlotsDragEvents();
        }

        protected override void InitializeSlots()
        {
            slots = new List<PossessionSlotUI>();
            for (var i = 0; i < playerInventory.GetSize(); i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, transform);
                inventorySlot.Setup(playerInventory, i);
                slots.Add(inventorySlot);
            }
        }

        private void DestroyAllSlots()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            UnRegisterPossessionSlotsDragEvents();
            slots = new List<PossessionSlotUI>();
        }

    }
}