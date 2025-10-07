using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.UI.Inventory
{
    public abstract class PossessionUI : MonoBehaviour
    {
        protected List<PossessionSlotUI> slots;

        public UnityEvent<bool> onDragging;
        
        protected abstract void InitializeSlots();

        protected void RegisterPossessionSlotsDragEvents()
        {
            if (slots == null || slots.Count == 0) return;

            foreach (var slot in slots)
            {
                slot.DragItem.onDragging.AddListener(HandleDragging);
            }
        }

        protected void UnRegisterPossessionSlotsDragEvents()
        {
            if (slots == null || slots.Count == 0) return;
            
            foreach (var slot in slots)
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