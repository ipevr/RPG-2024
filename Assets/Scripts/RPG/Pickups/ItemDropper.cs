using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Inventory;
using RPG.Saving;

namespace RPG.Pickups
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private readonly List<IInventoriable> droppedInventoriables = new();

        public void Drop(InventoryItem item, int amount)
        {
            SpawnPickup(item, GetDropLocation(), amount);
        }

        private Vector3 GetDropLocation()
        {
            return transform.position;
        }

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int amount)
        {
            var inventoriableItem = item.GetInventoriable();
            var spawnedItem = inventoriableItem.Spawn(spawnLocation);
            spawnedItem.Setup(item, amount);
            spawnedItem.OnPickupInventoriable.AddListener(HandlePickup);
            droppedInventoriables.Add(spawnedItem);
        }

        private void HandlePickup(IInventoriable inventoriablePickedUp)
        {
            IInventoriable inventoriableItemToBeRemoved = null;
            foreach (var inventoriable in droppedInventoriables)
            {
                if (inventoriable.Equals(inventoriablePickedUp))
                {
                    inventoriableItemToBeRemoved = inventoriablePickedUp;
                }
            }

            if (inventoriableItemToBeRemoved != null)
            {
                droppedInventoriables.Remove(inventoriableItemToBeRemoved);
            }
        }

        public JToken CaptureAsJToken()
        {
            var state = new JArray();
            IList<JToken> droppedItemsList = state;
            
            foreach (var drop in droppedInventoriables)
            {
                var dropState = new JObject();
                IDictionary<string, JToken> dropStateDict = dropState;
                
                dropStateDict.Add("itemId", drop.GetItem().ItemId);
                dropStateDict.Add("position", drop.GetPosition().ToToken());
                dropStateDict.Add("amount", drop.GetAmount());
                droppedItemsList.Add(dropState);
            }
            
            return state;
        }

        public void RestoreFromJToken(JToken state)
        {
            if (state is not JArray jArray) return;
            IList<JToken> droppedItemsList = jArray;
            
            foreach (var jToken in droppedItemsList)
            {
                if (jToken is not JObject jObject) continue;
                IDictionary<string, JToken> stateDict = jObject;
                
                var itemId = stateDict["itemId"].ToString();
                var position = stateDict["position"].ToVector3();
                var amount = (int)stateDict["amount"];
                
                SpawnPickup(InventoryItem.GetFromId(itemId) as ConsumableItem, position, amount);
                
            }
            Debug.Log("Restored dropped items");
        }
    }
}