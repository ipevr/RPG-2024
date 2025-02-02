using UnityEngine;

namespace RPG.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int inventorySlots = 4;

        public int GetInventorySlots()
        {
            return inventorySlots;
        }
    }
}