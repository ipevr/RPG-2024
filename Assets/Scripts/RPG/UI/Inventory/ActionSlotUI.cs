using System;
using UnityEngine;
using RPG.Inventory;
using UnityEngine.Rendering;

namespace RPG.UI.Inventory
{
    public class ActionSlotUI : PossessionSlotUI
    {
        [SerializeField] private int index;
        [SerializeField] private AmountDisplay amountDisplay;
        
        private PlayerActionStore actionStore;

        public override void Awake()
        {
            base.Awake();
            actionStore = PlayerActionStore.GetPlayerActionStore();
            actionStore.onActionStoreChanged.AddListener(RedrawUI);
        }

        private void Start()
        {
            RedrawUI();
        }

        public override InventoryItem GetItem()
        {
            return actionStore.GetAction(index);
        }

        public override int GetAmount()
        {
            return actionStore.GetAmount(index);
        }

        public override void RemoveItems(int number)
        {
            actionStore.RemoveItem(index);
        }

        public override void AddItems(InventoryItem item, int number)
        {
            actionStore.AddItem(item, number, index);
        }

        public override int MaxAcceptable(InventoryItem item)
        {
            return actionStore.MaxAcceptable(item, index);
        }

        private void RedrawUI()
        {
            icon.SetItem(GetItem());
            amountDisplay.SetAmount(GetAmount());
        }

    }
}