using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils.UI.Dragging;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// To be placed on icons representing the item in a slot. Allows the item to be dragged into other slots.
    /// </summary>
    public class PossessionDragItem : DragItem<InventoryItem>
    {
        public UnityEvent<bool> onBeginDrag;
        public UnityEvent<bool> onEndDrag;
        public UnityEvent<bool> onDragging;

        protected override void OnBeginDragHandler(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(Keyboard.current.shiftKey.isPressed);
            onDragging?.Invoke(true);
            base.OnBeginDragHandler(eventData);
        }

        protected override void OnEndDragHandler(PointerEventData eventData)
        {
            onEndDrag?.Invoke(Keyboard.current.shiftKey.isPressed);
            onDragging?.Invoke(false);
            base.OnEndDragHandler(eventData);
        }
    }
}