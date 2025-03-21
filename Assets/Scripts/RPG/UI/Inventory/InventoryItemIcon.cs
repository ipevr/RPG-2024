﻿using RPG.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// To be put on the inventory icon representing an inventory item. Allows the slot to update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        public void SetItem(InventoryItem item)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.Icon;
            }
        }
    }
}