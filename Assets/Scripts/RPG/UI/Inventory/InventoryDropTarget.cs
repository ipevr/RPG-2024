using UnityEngine;
using Utils.UI.Dragging;
using RPG.Inventory;
using RPG.Pickups;

namespace RPG.UI.Inventory
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }

        public void AddItems(InventoryItem item, int number)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().Drop(item);
        }
    }
}