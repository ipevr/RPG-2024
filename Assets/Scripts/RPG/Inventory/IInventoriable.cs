using UnityEngine;

namespace RPG.Inventory
{
    public interface IInventoriable
    {
        void Setup(InventoryItem inventoryItem);
        IInventoriable Spawn(Vector3 position);
    }
}