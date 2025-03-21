using System;
using UnityEngine;
using RPG.Inventory;

namespace RPG.Pickups
{
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] private InventoryItem inventoryItem;

        private void Start()
        {
            SpawnPickup();
        }

        private void SpawnPickup()
        {
            var pickup = inventoryItem.GetInventoriable();
            if (pickup == null)
            {
                Debug.LogError("IInventoriable is no pickup");
                return;
            }
            var spawnedPickup = pickup.Spawn(transform.position);
            spawnedPickup.Setup(inventoryItem);
        }
    }
}