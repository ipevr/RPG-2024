using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    /// <summary>
    /// To be put on the inventory icon representing an inventory item. Allows the slot to update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        public Sprite GetItem()
        {
            var iconImage = GetComponent<Image>();
            return iconImage.enabled ? iconImage.sprite : null;
        }

        public void SetItem(Sprite item)
        {
            var iconImage = GetComponent<Image>();
            iconImage.enabled = item != null;
            if (item != null)
            {
                iconImage.sprite = item;
            }
        }
    }
}