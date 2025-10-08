using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Inventory
{
    public interface IInventoriable
    {
        void Setup(InventoryItem inventoryItem, int amount);
        IInventoriable Spawn(Vector3 position, Transform parent);
        void Destroy();
        InventoryItem GetItem();
        int GetAmount();
        Vector3 GetPosition();
        UnityEvent<IInventoriable> OnPickupInventoriable { get; }
    }
}