using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{
    public class PlayerInventory : MonoBehaviour, ISaveable
    {
        [SerializeField] private int numberOfInventorySlots = 16;

        private struct InventorySlot
        {
            public InventoryItem InventoryItem;
            public int CurrentStackSize;
        }

        private InventorySlot[] slots;

        public event Action OnInventoryChanged;
        
        #region Unity Event Functions

        private void Awake()
        {
            slots = new InventorySlot[numberOfInventorySlots];
        }

        #endregion

        #region Public Methods
        
        public static PlayerInventory GetPlayerInventory()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerInventory>();
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindNonFullStackOrEmptySlot(item) >= 0;
        }

        public int GetSize()
        {
            return slots.Length;
        }

        public int AddToFirstAvailableSlot(InventoryItem item, int amount)
        {
            return AddItemsBeginningAtSlot(0, item, amount);
        }

        public bool HasItem(InventoryItem item)
        {
            foreach (var inventorySlot in slots)
            {
                if (ReferenceEquals(inventorySlot.InventoryItem, item)) return true;
            }
            
            return false;
        }

        public InventoryItem GetItemInSlot(int slotNumber)
        {
            return slots[slotNumber].InventoryItem;
        }

        public int GetNumberInSlot(int slotNumber)
        {
            return !slots[slotNumber].InventoryItem ? 0 : slots[slotNumber].CurrentStackSize;
        }

        public int RemoveFromSlot(int slotNumber, int amount)
        {
            var slot = slots[slotNumber];

            if (slot.InventoryItem == null) return 0;

            var removedAmount = Math.Min(amount, slot.CurrentStackSize);
            slot.CurrentStackSize -= removedAmount;

            if (slot.CurrentStackSize <= 0)
            {
                slot.InventoryItem = null;
                slot.CurrentStackSize = 0;
            }

            OnInventoryChanged?.Invoke();
            return removedAmount;
        }

        public int AddItemsBeginningAtSlot(int slotNumber, InventoryItem item, int amount)
        {
            var remainingAmount = amount;
            
            while (remainingAmount > 0)
            {
                var availableSpace = GetAvailableSpaceInSlot(slotNumber, item);

                if (availableSpace > 0)
                {
                    var amountToAdd = Mathf.Min(remainingAmount, availableSpace);
                    AddToSlot(slotNumber, item, amountToAdd);
                    remainingAmount -= amountToAdd;
                }

                if (remainingAmount > 0)
                {
                    slotNumber++;
                    if (slotNumber >= slots.Length) break;
                }
            }

            return amount - remainingAmount;
        }
        
        public int MaxAcceptable(InventoryItem item)
        {
            var totalAvailableSpace = 0;

            foreach (var slot in slots)
            {
                if (slot.InventoryItem == item)
                {
                    totalAvailableSpace += item.MaxStackSize - slot.CurrentStackSize;
                }
            }

            var emptySlotCount = 0;
            foreach (var slot in slots)
            {
                if (slot.InventoryItem == null)
                {
                    emptySlotCount++;
                }
            }

            totalAvailableSpace += emptySlotCount * item.MaxStackSize;

            return totalAvailableSpace;
        }
 
        #endregion

        #region Private Methods
        
        private int GetAvailableSpaceInSlot(int slotNumber, InventoryItem item)
        {
            if (!slots[slotNumber].InventoryItem) return item.MaxStackSize;
            if (slots[slotNumber].InventoryItem != item) return 0;
    
            return item.MaxStackSize - slots[slotNumber].CurrentStackSize;
        }
        
        private void AddToSlot(int slotNumber, InventoryItem item, int amount)
        {
            if (!slots[slotNumber].InventoryItem)
            {
                slots[slotNumber].InventoryItem = item;
            }

            slots[slotNumber].CurrentStackSize += amount;
            OnInventoryChanged?.Invoke();
        }

        private int FindNonFullStackOrEmptySlot(InventoryItem item)
        {
            var i = FindNonFullStack(item);

            if (i < 0)
            {
                i = FindEmptySlot();
            }
            
            return i;
        }
        
        private int FindEmptySlot()
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i].InventoryItem == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindNonFullStack(InventoryItem item)
        {
            if (item.MaxStackSize == 1) return -1;

            for (var i = 0; i < slots.Length; i++)
            {
                if (ReferenceEquals(slots[i].InventoryItem, item) && slots[i].CurrentStackSize < item.MaxStackSize)
                {
                    return i;
                };
            }
            
            return -1;
        }

        #endregion

        #region Interface Implementations

        private struct SlotState
        {
            public string ItemId;
            public int Amount;
        }

        JToken ISaveable.CaptureAsJToken()
        {
            var state = new JObject();
            IDictionary<string, JToken> slotContent = state;
            for (var i = 0; i < slots.Length; i++)
            {
                if (!slots[i].InventoryItem) continue;
                var slotToSave = new SlotState
                {
                    ItemId = slots[i].InventoryItem.ItemId,
                    Amount = slots[i].CurrentStackSize
                };
                slotContent.Add(i.ToString(), JToken.FromObject(slotToSave));
            }

            return state;
        }

        void ISaveable.RestoreFromJToken(JToken state)
        {
            if (state is not JObject jObject) return;
            
            IDictionary<string, JToken> stateDict = jObject;
            
            foreach (var slot in stateDict)
            {
                var index = int.Parse(slot.Key);
                var slotState = slot.Value.ToObject<SlotState>();
                var inventoryItem = InventoryItem.GetFromId(slotState.ItemId);
                AddItemsBeginningAtSlot(index, inventoryItem, slotState.Amount);
            }
        }

        #endregion
    }
}