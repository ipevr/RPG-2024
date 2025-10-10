using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Utils;
using RPG.Saving;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour, ISaveable
    {
        private LazyValue<Dictionary<EquipLocation, EquipableItem>> equippedItems;

        public UnityEvent onEquipmentChanged;

        private void Awake()
        {
            equippedItems = new LazyValue<Dictionary<EquipLocation, EquipableItem>>(InitializeSlots);
        }

        public static PlayerEquipment GetPlayerEquipment()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerEquipment>();
        }
        
        public void PutInSlot(EquipableItem item, EquipLocation location)
        {
            equippedItems.value[location] = item;
            onEquipmentChanged.Invoke();
        }
        
        public EquipableItem GetItemInSlot(EquipLocation location) 
        {
            return equippedItems.value[location];
        }

        public bool IsEquipped(EquipLocation location)
        {
            return equippedItems.value[location] != null;
        }

        public void RemoveFromSlot(EquipLocation location)
        {
            equippedItems.value[location] = null;
            onEquipmentChanged.Invoke();
        }

        private Dictionary<EquipLocation, EquipableItem> InitializeSlots()
        {
            var items = new Dictionary<EquipLocation, EquipableItem>();
            foreach (EquipLocation equipLocation in Enum.GetValues(typeof(EquipLocation)))
            {
                items.Add(equipLocation, null);
            }
            
            return items;
        }

        #region Interface Implementations

        JToken ISaveable.CaptureAsJToken()
        {
            var state = new JObject();
            IDictionary<string, JToken> equipmentContent = state;
            foreach (var item in equippedItems.value)
            {
                equipmentContent.Add(item.Key.ToString(), JToken.FromObject(item.Value ? item.Value.ItemId : string.Empty));
            }
            
            return state;
        }

        void ISaveable.RestoreFromJToken(JToken state)
        {
            if (state is not JObject jObject) return;
            
            IDictionary<string, JToken> stateDict = jObject;

            foreach (var item in stateDict)
            {
                var location = (EquipLocation)Enum.Parse(typeof(EquipLocation), item.Key);
                var itemId = item.Value.ToObject<string>();
                
                var itemInstance = string.IsNullOrEmpty(itemId) ? null : InventoryItem.GetFromId(itemId);
                PutInSlot(itemInstance as EquipableItem, location);
            }
        }

        #endregion
    }
}