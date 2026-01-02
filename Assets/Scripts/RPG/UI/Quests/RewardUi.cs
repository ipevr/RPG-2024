using RPG.Inventory;
using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class RewardUi : MonoBehaviour, IItemHolder
    {
        private InventoryItem item;

        public void Setup(InventoryItem inventoryItem)
        {
            item = inventoryItem;
            GetComponent<PossessionItemIcon>().SetItem(inventoryItem);
        }
        
        public InventoryItem GetItem()
        {
            return item;
        }
    }
}