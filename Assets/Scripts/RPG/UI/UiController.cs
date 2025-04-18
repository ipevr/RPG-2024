using System;
using RPG.Control;
using UnityEngine;
using RPG.UI.Inventory;

namespace RPG.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private InventoryUI inventoryUI;
        
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            inventoryUI.onDragging.AddListener(HandleInventoryUiDragging);
        }

        private void OnDisable()
        {
            inventoryUI.onDragging.RemoveListener(HandleInventoryUiDragging);
        }

        private void HandleInventoryUiDragging(bool isDragging)
        {
            playerController.AllowInteraction(!isDragging);
        }
        
    }
}