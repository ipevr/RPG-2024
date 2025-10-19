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

        protected override void Awake()
        {
            base.Awake();
            actionStore = PlayerActionStore.GetPlayerActionStore();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            actionStore.onActionStoreChanged.AddListener(RedrawUI);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            actionStore.onActionStoreChanged.RemoveListener(RedrawUI);
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
            actionStore.RemoveItems(index, number);
        }

        public override void AddItems(InventoryItem item, int number)
        {
            actionStore.AddAction(item, number, index);
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