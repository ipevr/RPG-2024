using RPG.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// To be put on the inventory icon representing an inventory item. Allows the slot to update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class PossessionItemIcon : MonoBehaviour
    {
        private InventoryItem item;
        
        public void SetItem(InventoryItem inventoryItem)
        {
            var iconImage = GetComponent<Image>();
            if (!inventoryItem)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = inventoryItem.Icon;
            }
        }
    }
}