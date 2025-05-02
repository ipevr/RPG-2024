using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Inventory;
using RPG.Saving;

namespace RPG.Pickups
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] private InventoryItem inventoryItem;
        [SerializeField] private int amount = 1;

        private IInventoriable spawnedPickup;
        private int amountPickedUp;

        private void Awake()
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
            spawnedPickup = pickup.Spawn(transform.position);
            spawnedPickup.Setup(inventoryItem, amount);
            spawnedPickup.OnPickupInventoriable.AddListener(HandlePickedUp);
        }

        private void DestroyPickup()
        {
            spawnedPickup?.Destroy();
        }

        private void HandlePickedUp(IInventoriable inventoriablePickedUp)
        {
            amountPickedUp = inventoriablePickedUp.GetAmount();
        }

        public JToken CaptureAsJToken()
        {
            return amountPickedUp;
        }

        public void RestoreFromJToken(JToken state)
        {
            amount = (int)state;
            if (amount <= 0)
            {
                DestroyPickup();
            }
        }
    }
}