using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Inventory
{
    public class PlayerActionStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, DockedItemSlot> dockedItems = new ();
        
        private class DockedItemSlot
        {
            public ActionItem item;
            public int amount;
        }
        
        public UnityEvent onActionStoreChanged;

        public static PlayerActionStore GetPlayerActionStore()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerActionStore>();
        }

        public void AddItem(InventoryItem item, int amount, int index)
        {
            if (dockedItems.ContainsKey(index) && ReferenceEquals(dockedItems[index].item, item))
            {
                dockedItems[index].amount += amount;
            }
            else
            {
                var dockedItem = new DockedItemSlot
                {
                    item = item as ActionItem,
                    amount = amount
                };
                dockedItems.Add(index, dockedItem);
            }

            onActionStoreChanged?.Invoke();
        }

        public ActionItem GetAction(int index)
        {
            return dockedItems.TryGetValue(index, out var item) ? item.item : null;
        }

        public int GetAmount(int index)
        {
            return dockedItems.TryGetValue(index, out var item) ? item.amount : 0;
        }
        
        public void RemoveItem(int index)
        {
            dockedItems.Remove(index);

            onActionStoreChanged?.Invoke();
        }

        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            
            if (!actionItem) return 0;

            if (dockedItems.ContainsKey(index) && !ReferenceEquals(dockedItems[index].item, actionItem))
            {
                return 0;
            }

            return actionItem.IsConsumable ? int.MaxValue : 1;
        }

        #region Interface Implementations
        
        private struct SlotState
        {
            public string itemId;
            public int amount;
        }

        public JToken CaptureAsJToken()
        {
            var state = new JObject();
            IDictionary<string, JToken> actionStoreContent = state;
            foreach (var item in dockedItems)
            {
                var slotToSave = new SlotState
                {
                    itemId = item.Value.item.ItemId,
                    amount = item.Value.amount
                };
                
                actionStoreContent.Add(item.Key.ToString(), JToken.FromObject(slotToSave));
            }
            
            return state;
        }

        public void RestoreFromJToken(JToken state)
        {
            if (state is not JObject jObject) return;
            
            IDictionary<string, JToken> stateDict = jObject;
            dockedItems.Clear();
            
            foreach (var slot in stateDict)
            {
                var slotState = slot.Value.ToObject<SlotState>();
                var item = InventoryItem.GetFromId(slotState.itemId);
                AddItem(item, slotState.amount, int.Parse(slot.Key));
            }
        }
        
        #endregion
    }
}