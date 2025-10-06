using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour
    {
        private Dictionary<EquipLocation, IEquipableItem> slotDictionary;

        private void Awake()
        {
            InitializeSlots();
        }

        public static PlayerEquipment GetPlayerEquipment()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerEquipment>();
        }
        
        public void PutInSlot(IEquipableItem item, EquipLocation location)
        {
            slotDictionary[location] = item;
        }
        
        public IEquipableItem GetItemInSlot(EquipLocation location) 
        {
            return slotDictionary[location];
        }

        public bool IsEquipped(EquipLocation location)
        {
            return slotDictionary[location] != null;
        }

        public void RemoveFromSlot(EquipLocation location)
        {
            slotDictionary[location] = null;
        }

        private void InitializeSlots()
        {
            slotDictionary = new Dictionary<EquipLocation, IEquipableItem>();
            foreach (EquipLocation equipLocation in Enum.GetValues(typeof(EquipLocation)))
            {
                slotDictionary.Add(equipLocation, null);
            }
        }

    }
}