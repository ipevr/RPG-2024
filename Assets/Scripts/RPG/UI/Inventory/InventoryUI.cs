using System;
using UnityEngine;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class InventoryUI : MonoBehaviour
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

            for (var i = 0; i < playerInventory.GetSize(); i++)
            {
                var itemUi = Instantiate(inventorySlotPrefab, transform);
                itemUi.Setup(playerInventory, i);
            }
        }

        private void DestroyAllSlots()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}