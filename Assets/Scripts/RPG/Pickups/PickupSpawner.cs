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

        private IInventoriable spawnedPickup;
        private bool pickedUp;

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
            spawnedPickup.Setup(inventoryItem);
            spawnedPickup.OnPickupInventoriable.AddListener(HandlePickedUp);
        }

        private void DestroyPickup()
        {
            spawnedPickup?.Destroy();
        }

        private void HandlePickedUp(IInventoriable inventoriablePickedUp)
        {
            pickedUp = true;
        }

        public JToken CaptureAsJToken()
        {
            return pickedUp;
        }

        public void RestoreFromJToken(JToken state)
        {
            pickedUp = (bool)state;
            if (pickedUp)
            {
                DestroyPickup();
            }
        }
    }
}