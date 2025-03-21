using UnityEngine;
using RPG.Inventory;

namespace RPG.Pickups
{
    public class ItemDropper : MonoBehaviour
    {
        public void Drop(InventoryItem item)
        {
            Debug.Log("Dropping " + item.name);
            var droppedItem = item.GetInventoriable();
            var spawnedItem = droppedItem.Spawn(transform.position);
            spawnedItem.Setup(item);
        }
    }
}