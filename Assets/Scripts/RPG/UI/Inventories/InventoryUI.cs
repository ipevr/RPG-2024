using UnityEngine;
using RPG.Inventory;

namespace RPG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab;

        private PlayerInventory inventory;
        
        private void Awake()
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            var numberOfSlots = inventory.GetInventorySlots();

            for (var i = 0; i < numberOfSlots; i++)
            {
                Instantiate(inventorySlotPrefab, transform);
            }
        }
    }
}