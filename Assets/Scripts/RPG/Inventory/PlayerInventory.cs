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
            return FindSlot(item) >= 0;
        }

        public int GetSize()
        {
            return slots.Length;
        }

        public bool AddToFirstAvailableSlot(InventoryItem item)
        {
            var i = FindSlot(item);
            if (i < 0) return false;

            slots[i].InventoryItem = item;
            slots[i].CurrentStackSize++;

            OnInventoryChanged?.Invoke();
            
            return true;
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
            if (slots[slotNumber].InventoryItem == null) return 0;
            
            if (amount > slots[slotNumber].CurrentStackSize)
            {
                var stackSize = slots[slotNumber].CurrentStackSize;
                slots[slotNumber].InventoryItem = null;
                slots[slotNumber].CurrentStackSize = 0;
                OnInventoryChanged?.Invoke();
                return stackSize;
            }
            
            slots[slotNumber].CurrentStackSize -= amount;
            if (slots[slotNumber].CurrentStackSize <= 0)
            {
                slots[slotNumber].InventoryItem = null;
            }
            OnInventoryChanged?.Invoke();
            return amount;
        }

        public bool AddItemsToSlot(int slotNumber, InventoryItem item, int amount)
        {
            while (true)
            {
                if (amount == 0) return true;

                if (slots[slotNumber].InventoryItem == item || slots[slotNumber].InventoryItem == null)
                {
                    var availableSpace = slots[slotNumber].InventoryItem == null
                        ? item.MaxStackSize
                        : item.MaxStackSize - slots[slotNumber].CurrentStackSize;

                    var amountToAdd = Mathf.Min(amount, availableSpace);

                    if (slots[slotNumber].InventoryItem == null)
                    {
                        slots[slotNumber].InventoryItem = item;
                    }

                    slots[slotNumber].CurrentStackSize += amountToAdd;
                    amount -= amountToAdd;

                    OnInventoryChanged?.Invoke();

                    if (amount == 0) return true;
                }

                var nextSlotNumber = FindEmptySlot();

                if (nextSlotNumber == -1)
                {
                    return false;
                }

                slotNumber = nextSlotNumber;
            }
        }
 
        #endregion

        #region Private Methods

        private int FindSlot(InventoryItem item)
        {
            var i = FindStack(item);

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

        private int FindStack(InventoryItem item)
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
                AddItemsToSlot(index, inventoryItem, slotState.Amount);
            }
        }

        #endregion
    }
}