using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Saving;
using Utils;

namespace RPG.Inventory
{
    public class PlayerInventory : MonoBehaviour, ISaveable
    {
        [SerializeField] private int numberOfInventorySlots = 16;

        private LazyValue<InventorySlot[]> slots;

        public event Action OnInventoryChanged;
        
        #region Unity Event Functions

        private void Awake()
        {
            slots = new LazyValue<InventorySlot[]>(InitializeSlots);
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
            return slots.value.Length;
        }

        public int AddToFirstAvailableSlot(InventoryItem item, int amount)
        {
            var index = FindNonFullStack(item);
            return AddItemsBeginningAtSlot(index == -1 ? 0 : index, item, amount);
        }

        public bool HasItem(InventoryItem item)
        {
            foreach (var inventorySlot in slots.value)
            {
                if (ReferenceEquals(inventorySlot.Item, item)) return true;
            }
            
            return false;
        }

        public InventoryItem GetItemInSlot(int slotNumber)
        {
            return slots.value[slotNumber].Item;
        }

        public int GetAmountInSlot(int slotNumber)
        {
            return !slots.value[slotNumber].Item ? 0 : slots.value[slotNumber].CurrentStackSize;
        }

        public int RemoveFromSlot(int slotNumber, int amount)
        {
            if (!slots.value[slotNumber].Item) return 0;

            var removedAmount = slots.value[slotNumber].RemoveFromStack(amount);

            OnInventoryChanged?.Invoke();
            return removedAmount;
        }

        public int AddItemsBeginningAtSlot(int slotNumber, InventoryItem item, int amount)
        {
            var remainingAmount = amount;
            
            while (remainingAmount > 0)
            {
                var availableSpace = slots.value[slotNumber].GetRemainingSpace(item);

                if (availableSpace > 0)
                {
                    var amountToAdd = Mathf.Min(remainingAmount, availableSpace);
                    AddToSlot(slotNumber, item, amountToAdd);
                    remainingAmount -= amountToAdd;
                }

                if (remainingAmount > 0)
                {
                    slotNumber++;
                    if (slotNumber >= slots.value.Length) break;
                }
            }

            return amount - remainingAmount;
        }
        
        public int MaxAcceptable(InventoryItem item)
        {
            var totalAvailableSpace = 0;

            foreach (var slot in slots.value)
            {
                totalAvailableSpace += slot.GetRemainingSpace(item);
            }

            return totalAvailableSpace;
        }
 
        #endregion

        #region Private Methods

        private InventorySlot[] InitializeSlots()
        {
            var inventorySlots = new InventorySlot[numberOfInventorySlots];
            for (var i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i] = new InventorySlot();
            }
            
            return inventorySlots;
        }
        
        private void AddToSlot(int slotNumber, InventoryItem item, int amount)
        {
            if (!slots.value[slotNumber].Item)
            {
                slots.value[slotNumber] = new InventorySlot(item, amount);
            }
            else
            {
                slots.value[slotNumber].AddToStack(amount);
            }

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
            for (var i = 0; i < slots.value.Length; i++)
            {
                if (!slots.value[i].Item)
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindNonFullStack(InventoryItem item)
        {
            for (var i = 0; i < slots.value.Length; i++)
            {
                if (slots.value[i].Item == item && !slots.value[i].HasFullStack())
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
            public string itemId;
            public int amount;
        }

        JToken ISaveable.CaptureAsJToken()
        {
            var state = new JObject();
            IDictionary<string, JToken> slotContent = state;
            for (var i = 0; i < slots.value.Length; i++)
            {
                if (!slots.value[i].Item) continue;
                var slotToSave = new SlotState
                {
                    itemId = slots.value[i].Item.ItemId,
                    amount = slots.value[i].CurrentStackSize
                };
                slotContent.Add(i.ToString(), JToken.FromObject(slotToSave));
            }

            return state;
        }

        void ISaveable.RestoreFromJToken(JToken state)
        {
            if (state is not JObject jObject) return;
            
            IDictionary<string, JToken> stateDict = jObject;
            slots.value = InitializeSlots();
            
            foreach (var slot in stateDict)
            {
                var index = int.Parse(slot.Key);
                var slotState = slot.Value.ToObject<SlotState>();
                var inventoryItem = InventoryItem.GetFromId(slotState.itemId);
                AddItemsBeginningAtSlot(index, inventoryItem, slotState.amount);
            }
            
            OnInventoryChanged?.Invoke();
        }

        #endregion
    }
}