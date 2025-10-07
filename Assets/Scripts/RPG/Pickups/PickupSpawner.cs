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

        private void Awake()
        {
            SpawnPickup();
            remainingAmount = amount;
        }

        private void SpawnPickup()
        {
            var pickup = inventoryItem.GetInventoriable();
            if (pickup == null)
            {
                Debug.LogError("IInventoriable is no pickup");
                return;
            }
            spawnedPickup = pickup.Spawn(transform.position);
            spawnedPickup.Setup(inventoryItem, amount);
            spawnedPickup.OnPickupInventoriable.AddListener(HandlePickedUp);
        }

        private void DestroyPickup()
        {
            spawnedPickup?.Destroy();
            spawnedPickup?.OnPickupInventoriable.RemoveListener(HandlePickedUp);
            spawnedPickup = null;
        }

        private void HandlePickedUp(IInventoriable inventoriablePickedUp)
        {
            remainingAmount = inventoriablePickedUp.GetAmount();
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
                return;
            }
            
            amount = remainingAmount;
            SpawnPickup();
        }
    }
}