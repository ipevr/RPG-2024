using System;
using UnityEngine;
using Utils.UI.Dragging;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public abstract class PossessionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] protected PossessionItemIcon icon;
        [SerializeField] protected PossessionDragItem dragItem;
        
        public PossessionDragItem DragItem => dragItem;


        private void OnEnable()
        {
            dragItem.onDragging.AddListener(HandleDragging);
            dragItem.onBeginDrag.AddListener(HandleBeginDrag);
            dragItem.onEndDrag.AddListener(HandleEndDrag);
        }

        private void OnDisable()
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
        protected abstract void HandleBeginDrag(bool singleMode);

        protected abstract void HandleEndDrag(bool singleMode);

        protected abstract void HandleDragging(bool dragging);
    }
}