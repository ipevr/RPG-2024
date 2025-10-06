using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.UI.Inventory
{
    public abstract class PossessionUI : MonoBehaviour
    {
        protected List<PossessionSlotUI> possessionSlots;

        public UnityEvent<bool> onDragging;
        
        protected abstract void InitializeSlots();

        protected void RegisterPossessionSlotsDragEvents()
        {
            if (possessionSlots == null || possessionSlots.Count == 0) return;

            foreach (var slot in possessionSlots)
            {
                slot.DragItem.onDragging.AddListener(HandleDragging);
            }
        }

        protected void UnRegisterPossessionSlotsDragEvents()
        {
            if (possessionSlots == null || possessionSlots.Count == 0) return;
            
            foreach (var slot in possessionSlots)
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