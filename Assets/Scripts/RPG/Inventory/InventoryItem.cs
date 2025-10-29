using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID for saving / loading. Clear this field if you want to generate a new one.")]
        [SerializeField] private string itemId;
        [Tooltip("Item name to be displayed in UI.")] 
        [SerializeField] private string displayName;
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] private string description;
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] private Sprite icon;
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private bool isStackable;

        private static Dictionary<string, InventoryItem> itemLookupCache;

        public string ItemId => itemId;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool IsStackable => isStackable;

        public static InventoryItem GetFromId(string itemId)
        {
            if (itemLookupCache == null)
            {
                CreateItemLookupCache();
            }
            
            if (itemId == null || !itemLookupCache!.TryGetValue(itemId, out var possessionItem)) return null;
            
            return possessionItem;
        }

        public IInventoriable GetPickup()
        {
            var inventoriable = pickupPrefab.GetComponent<IInventoriable>();

            if (inventoriable == null)
            {
                Debug.LogError($"{pickupPrefab.name} does not have any Inventoriable");
            }
            
            return inventoriable;
        }

        private static void CreateItemLookupCache()
        {
            itemLookupCache = new Dictionary<string, InventoryItem>();
            var itemList = Resources.LoadAll<InventoryItem>("");
            foreach (var item in itemList)
            {
                if (itemLookupCache.TryGetValue(item.itemId, out var inventoryItem))
                {
                    Debug.LogError($"There is a duplicate InventoryItem Id for objects: {item} and {inventoryItem}");
                    continue;
                }
                
                itemLookupCache[item.itemId] = item;
            }
        }

        #region Interface Implementations

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                itemId = Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
        
        #endregion
    }
}