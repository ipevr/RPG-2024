using System;
using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerEquipment : MonoBehaviour
    {
        private EquipmentSlot[] slots;

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
            slots[(int) location].SetItem(item);
        }
        
        public IEquipableItem GetItemInSlot(EquipLocation location) 
        {
            return slots[(int) location].Item;
        }

        public bool IsEquipped(EquipLocation location)
        {
            return slots[(int) location].Item != null;
        }

        public void RemoveFromSlot(EquipLocation location)
        {
            slots[(int) location].RemoveItem();
        }

        private void InitializeSlots()
        {
            slots = new EquipmentSlot[Enum.GetNames(typeof(EquipLocation)).Length];
            for (var i = 0; i < slots.Length; i++)
            {
                slots[i] = new EquipmentSlot();
            }
        }

    }
}