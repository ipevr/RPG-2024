using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Inventory
{
    public interface IInventoriable
    {
        void Setup(InventoryItem inventoryItem);
        IInventoriable Spawn(Vector3 position);
        void Destroy();
        InventoryItem GetItem();
        Vector3 GetPosition();
        UnityEvent<IInventoriable> OnPickupInventoriable { get; }
    }
}