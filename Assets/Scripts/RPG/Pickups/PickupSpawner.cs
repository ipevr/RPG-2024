using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Inventory;
using RPG.Saving;
using UnityEngine.Serialization;

namespace RPG.Pickups
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] private InventoryItem inventoryItem;
        [SerializeField] private int amount = 1;

        private IInventoriable spawnedPickup;
        private int remainingAmount;
        private bool pickupIsDestroyed;

        private void Awake()
        {
            if (pickupIsDestroyed) return;
            
            SpawnPickup();
            remainingAmount = amount;
        }

        private void SpawnPickup()
        {
            if (amount == 0) return;
            
            var pickup = inventoryItem.GetInventoriable();
            if (pickup == null)
            {
                Debug.LogError("IInventoriable is no pickup");
                return;
            }
            spawnedPickup = pickup.Spawn(transform.position);
            spawnedPickup.Setup(inventoryItem, amount);
            spawnedPickup.OnPickupInventoriable.AddListener(HandlePickedUp);
            pickupIsDestroyed = false;
        }

        private void DestroyPickup()
        {
            if (pickupIsDestroyed) return;
            
            spawnedPickup?.Destroy();
            spawnedPickup?.OnPickupInventoriable.RemoveListener(HandlePickedUp);
            spawnedPickup = null;
            pickupIsDestroyed = true;
        }

        private void HandlePickedUp(IInventoriable inventoriablePickedUp)
        {
            remainingAmount = inventoriablePickedUp.GetAmount();
            if (remainingAmount <= 0)
            {
                DestroyPickup();
            }
        }

        public JToken CaptureAsJToken()
        {
            return remainingAmount;
        }

        public void RestoreFromJToken(JToken state)
        {
            DestroyPickup();
            remainingAmount = (int)state;
            if (remainingAmount <= 0)
            {
                pickupIsDestroyed = true;
                return;
            }
            
            amount = remainingAmount;
            SpawnPickup();
        }
    }
}