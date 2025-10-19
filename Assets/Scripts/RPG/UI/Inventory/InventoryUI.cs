using System;
using System.Collections;
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

        private void Awake()
        {
            playerInventory = PlayerInventory.GetPlayerInventory();
        }

        private void OnEnable()
        {
            playerInventory.onInventoryChanged.AddListener(Redraw);
            StartCoroutine(RedrawOnNextFrame());
        }

        private void OnDisable()
        {
            playerInventory.onInventoryChanged.RemoveListener(Redraw);
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
        }

        private void InitializeSlots()
        {
            for (var i = 0; i < playerInventory.GetSize(); i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, transform);
                inventorySlot.Setup(playerInventory, i);
            }
        }

        private IEnumerator RedrawOnNextFrame()
        {
            // This might be necessary as OnEnable might be called before Awake in PlayerInventory.
            yield return new WaitForEndOfFrame();
            Redraw();
        }
    }
}