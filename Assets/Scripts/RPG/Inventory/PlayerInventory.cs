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
        
        private InventoryItem[] slots;

        public event Action OnInventoryChanged;
        
        #region Unity Event Functions

        private void Awake()
        {
            slots = new InventoryItem[numberOfInventorySlots];
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

            slots[i] = item;

            OnInventoryChanged?.Invoke();
            
            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            foreach (var inventorySlot in slots)
            {
                if (ReferenceEquals(inventorySlot, item)) return true;
            }
            
            return false;
        }

        public InventoryItem GetItemInSlot(int slotNumber)
        {
            return slots[slotNumber];
        }

        public int GetNumberInSlot(int slotNumber)
        {
            return slots[slotNumber] == null ? 0 : 1;
        }

        public void RemoveFromSlot(int slotNumber)
        {
            slots[slotNumber] = null;
            
            OnInventoryChanged?.Invoke();
        }

        public bool AddItemToSlot(int slotNumber, InventoryItem item)
        {
            if (slots[slotNumber])
            {
                return AddToFirstAvailableSlot(item);
            }
            
            slots[slotNumber] = item;
            
            OnInventoryChanged?.Invoke();
            
            return true;
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
                if (slots[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindStack(InventoryItem item)
        {
            if (!item.Stackable) return -1;

            for (var i = 0; i < slots.Length; i++)
            {
                if (ReferenceEquals(slots[i], item))
                {
                    return i;
                };
            }
            
            return -1;
        }

        #endregion

        #region Interface Implementations

        JToken ISaveable.CaptureAsJToken()
        {
            var state = new JObject();
            IDictionary<string, JToken> slotContent = state;
            for (var i = 0; i < slots.Length; i++)
            {
                if (!slots[i]) continue;
                slotContent.Add(i.ToString(), slots[i].ItemId);
            }

            return state;
        }

        void ISaveable.RestoreFromJToken(JToken state)
        {
            if (state is not JObject jObject) return;
            
            IDictionary<string, JToken> stateDict = jObject;
            
            foreach (var slotState in stateDict)
            {
                var index = int.Parse(slotState.Key);
                var inventoryItem = InventoryItem.GetFromId(slotState.Value.ToObject<string>());
                AddItemToSlot(index, inventoryItem);
            }
        }

        #endregion
    }
}