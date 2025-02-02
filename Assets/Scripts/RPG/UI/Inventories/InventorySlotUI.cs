using UnityEngine;
using Utils.UI.Dragging;

namespace RPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite>
    {
        [SerializeField] private InventoryItemIcon icon;

        // Will be detailed when using stackable items. At the moment it says put in as much as you want if there is
        // still nothing, otherwise if anything is already in, nothing more is accepted.
        public int MaxAcceptable(Sprite item)
        {
            return GetItem() == null ? int.MaxValue : 0;
        }

        public void AddItems(Sprite item, int number)
        {
            // number will be used later for stackable items
            Debug.Log($"Adding items to inventory slot: ${gameObject.name}");
            icon.SetItem(item);
        }

        public Sprite GetItem()
        {
            Debug.Log($"Getting item from inventory slot: ${gameObject.name}");
            return icon.GetItem();
        }

        public int GetNumber()
        {
            return 1;
        }

        public void RemoveItems(int number)
        {
            Debug.Log($"Removing item from inventory slot: ${gameObject.name}");
            icon.SetItem(null);
        }
    }
}