using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour, ISaveable
    {
        private readonly Dictionary<EquipLocation, EquipableItem> equippedItems = new();

        public UnityEvent onEquipmentChanged;


        public static PlayerEquipment GetPlayerEquipment()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerEquipment>();
        }
        
        public void AddItem(EquipableItem item, EquipLocation location)
        {
            Debug.Assert(item.EquipLocation == location);
            
            equippedItems.Add(location, item);
            onEquipmentChanged?.Invoke();
        }
        
        public EquipableItem GetItem(EquipLocation location)
        {
            return equippedItems.GetValueOrDefault(location);
        }

        public bool IsEquipped(EquipLocation location)
        {
            return equippedItems.ContainsKey(location);
        }

        public void RemoveItem(EquipLocation location)
        {
            equippedItems.Remove(location);
            onEquipmentChanged?.Invoke();
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
            foreach (var item in equippedItems)
            {
                equipmentContent.Add(item.Key.ToString(), JToken.FromObject(item.Value.ItemId));
            }
            
            return state;
        }

        void ISaveable.RestoreFromJToken(JToken state)
        {
            if (state is not JObject jObject) return;
            
            IDictionary<string, JToken> stateDict = jObject;
            equippedItems.Clear();

            foreach (var item in stateDict)
            {
                var location = (EquipLocation)Enum.Parse(typeof(EquipLocation), item.Key);
                var itemId = item.Value.ToObject<string>();
                
                var itemInstance = InventoryItem.GetFromId(itemId);
                AddItem(itemInstance as EquipableItem, location);
            }
        }

        #endregion
    }
}