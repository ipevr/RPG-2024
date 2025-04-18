using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utils.UI.Dragging;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// To be placed on icons representing the item in a slot. Allows the item to be dragged into other slots.
    /// </summary>
    public class InventoryDragItem : DragItem<InventoryItem>
    {
        public UnityEvent onBeginDragEvent;
        public UnityEvent onEndDragEvent;

        protected override void OnBeginDragHandler(PointerEventData eventData)
        {
            onBeginDragEvent?.Invoke();
            base.OnBeginDragHandler(eventData);
        }

        protected override void OnEndDragHandler(PointerEventData eventData)
        {
            onEndDragEvent?.Invoke();
            base.OnEndDragHandler(eventData);
        }
    }
}