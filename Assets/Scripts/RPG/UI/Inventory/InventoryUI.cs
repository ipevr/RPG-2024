using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySlotUI inventorySlotPrefab;

        public UnityEvent<bool> onDragging;
        
        private PlayerInventory playerInventory;
        private List<InventorySlotUI> inventorySlots;
        
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

            for (var i = 0; i < playerInventory.GetSize(); i++)
            {
                var itemUi = Instantiate(inventorySlotPrefab, transform);
                itemUi.Setup(playerInventory, i);
                inventorySlots.Add(itemUi);
            }

            RegisterInventorySlotsDragEvents();
        }

        private void DestroyAllSlots()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            UnRegisterInventorySlotsDragEvents();
            inventorySlots = new List<InventorySlotUI>();
        }

        private void RegisterInventorySlotsDragEvents()
        {
            if (inventorySlots == null || inventorySlots.Count == 0) return;

            foreach (var slot in inventorySlots)
            {
                slot.DragItem.onDragging.AddListener(HandleDragging);
            }
        }

        private void UnRegisterInventorySlotsDragEvents()
        {
            if (inventorySlots == null || inventorySlots.Count == 0) return;
            
            foreach (var slot in inventorySlots)
            {
                slot.DragItem.onDragging.RemoveListener(HandleDragging);
            }
        }

        private void HandleDragging(bool status)
        {
            onDragging?.Invoke(status);
        }
    }
}