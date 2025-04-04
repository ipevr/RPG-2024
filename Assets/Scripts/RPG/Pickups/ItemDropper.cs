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
            var spawnedItem = droppedItem.Spawn(GetDropLocation());
            spawnedItem.Setup(item);
        }

        private Vector3 GetDropLocation()
        {
            return transform.position;
        }

    }
}