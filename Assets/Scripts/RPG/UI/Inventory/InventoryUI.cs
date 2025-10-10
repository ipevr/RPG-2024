using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySlotUI inventorySlotPrefab;

        private PlayerInventory playerInventory;
        private List<PossessionSlotUI> slots;

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
        }

        private void DestroyAllSlots()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            slots = new List<PossessionSlotUI>();
        }

        private void InitializeSlots()
        {
            slots = new List<PossessionSlotUI>();
            for (var i = 0; i < playerInventory.GetSize(); i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, transform);
                inventorySlot.Setup(playerInventory, i);
                slots.Add(inventorySlot);
            }
        }
    }
}