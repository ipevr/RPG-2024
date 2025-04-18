using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventory;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
                slot.DragItem.onBeginDragEvent.AddListener(HandleBeginDrag);
                slot.DragItem.onEndDragEvent.AddListener(HandleEndDrag);
            }
        }

        private void UnRegisterInventorySlotsDragEvents()
        {
            if (inventorySlots == null || inventorySlots.Count == 0) return;
            
            foreach (var slot in inventorySlots)
            {
                slot.DragItem.onBeginDragEvent.RemoveListener(HandleBeginDrag);
                slot.DragItem.onEndDragEvent.RemoveListener(HandleEndDrag);
            }
        }

        private void HandleBeginDrag()
        {
            onDragging?.Invoke(true);
        }

        private void HandleEndDrag()
        {
            onDragging?.Invoke(false);
        }

    }
}