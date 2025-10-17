using System;
using UnityEngine;
using UnityEngine.Events;
using Utils.UI.Dragging;
using RPG.Control;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public abstract class PossessionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] protected PossessionItemIcon icon;
        [SerializeField] protected PossessionDragItem dragItem;
        
        public PossessionDragItem DragItem => dragItem;
        public UnityEvent<bool> onDragging;

        private PlayerController playerController;

        protected virtual void Awake()
        {
            playerController = PlayerController.GetPlayerController();
        }

        protected virtual void OnEnable()
        {
            dragItem.onDragging.AddListener(HandleDragging);
            dragItem.onBeginDrag.AddListener(HandleBeginDrag);
            dragItem.onEndDrag.AddListener(HandleEndDrag);
        }

        protected virtual void OnDisable()
        {
            dragItem.onDragging.RemoveListener(HandleDragging);
            dragItem.onBeginDrag.RemoveListener(HandleBeginDrag);
            dragItem.onEndDrag.RemoveListener(HandleEndDrag);
        }

        public abstract InventoryItem GetItem();
        public abstract void AddItems(InventoryItem item, int number);
        public abstract int MaxAcceptable(InventoryItem item);
        public abstract int GetAmount();
        public abstract void RemoveItems(int number);
        protected virtual void HandleBeginDrag(bool singleMode){}

        protected virtual void HandleEndDrag(bool singleMode){}

        protected virtual void HandleDragging(bool dragging)
        {
            playerController.AllowInteraction(!dragging);
        }
    }
}