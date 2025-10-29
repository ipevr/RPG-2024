using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Inventory;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.Pickups
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private readonly List<DropRecord> dropRecords = new();
        
        private class DropRecord
        {
            public InventoryItem item;
            public int amount;
            public Vector3 dropLocation;
            public int sceneBuildIndex;
            public IInventoriable spawnedPickup;

            public bool IsFromThisScene()
            {
                return sceneBuildIndex == SceneManager.GetActiveScene().buildIndex;
            }
        }

        public void Drop(InventoryItem item, int amount)
        {
            var record = new DropRecord
            {
                item = item,
                amount = amount,
                dropLocation = GetDropLocation(),
                sceneBuildIndex = SceneManager.GetActiveScene().buildIndex
            };
            dropRecords.Add(record);
            SpawnPickup(record);
        }

        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        private void SpawnPickup(DropRecord record)
        {
            if (!record.IsFromThisScene()) return;
            
            var inventoriableItem = record.item.GetPickup();
            var spawnedItem = inventoriableItem.Spawn(record.dropLocation);
            record.spawnedPickup = spawnedItem;
            spawnedItem.Setup(record.item, record.amount);
            spawnedItem.OnPickupInventoriable.AddListener(HandlePickup);
        }

        private void HandlePickup(IInventoriable inventoriablePickedUp)
        {
            DropRecord recordToBeRemoved = null;
            foreach (var record in dropRecords)
            {
                if (!record.IsFromThisScene()) continue;
                
                if (record.spawnedPickup.Equals(inventoriablePickedUp))
                {
                    recordToBeRemoved = record;
                }
            }

            if (recordToBeRemoved != null)
            {
                dropRecords.Remove(recordToBeRemoved);
            }
        }

        private void DestroyAllDrops()
        {
            DestroyAllScenePickups();
            dropRecords.Clear();
        }

        private void DestroyAllScenePickups()
        {
            foreach (var record in dropRecords.Where(record => record.IsFromThisScene()))
            {
                record.spawnedPickup.Destroy();
            }
        }

        public JToken CaptureAsJToken()
        {
            var state = new JArray();
            IList<JToken> droppedItemsList = state;
            
            foreach (var drop in dropRecords)
            {
                var dropState = new JObject();
                IDictionary<string, JToken> dropStateDict = dropState;
                
                dropStateDict.Add("itemId", drop.item.ItemId);
                dropStateDict.Add("position", drop.dropLocation.ToToken());
                dropStateDict.Add("amount", drop.amount);
                dropStateDict.Add("sceneBuildIndex", drop.sceneBuildIndex);
                droppedItemsList.Add(dropState);
            }
            
            return state;
        }

        public void RestoreFromJToken(JToken state)
        {
            if (state is not JArray jArray) return;
            IList<JToken> droppedItemsList = jArray;
            
            DestroyAllDrops();
            
            foreach (var jToken in droppedItemsList)
            {
                if (jToken is not JObject jObject) continue;
                IDictionary<string, JToken> stateDict = jObject;
                
                var sceneBuildIndex = (int)stateDict["sceneBuildIndex"];
                var itemId = stateDict["itemId"].ToString();
                var position = stateDict["position"].ToVector3();
                var amount = (int)stateDict["amount"];

                var record = new DropRecord
                {
                    sceneBuildIndex = sceneBuildIndex,
                    item = InventoryItem.GetFromId(itemId),
                    amount = amount,
                    dropLocation = position,
                    spawnedPickup = null
                };
                dropRecords.Add(record);

                SpawnPickup(record);
                
            }
            Debug.Log("Restored dropped items");
        }
    }
}