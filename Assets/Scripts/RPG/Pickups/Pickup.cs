using System;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;
using RPG.Movement;

namespace RPG.Pickups
{
    public class Pickup : MonoBehaviour, IInventoriable
    {
        [SerializeField] private UnityEvent onPickup;

        private InventoryItem inventoryItem;
        private PlayerInventory playerInventory;
        private Mover playerMover;

        #region Unity Event Functions

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerInventory = player.GetComponent<PlayerInventory>();
            playerMover = player.GetComponent<Mover>();
        }

        #endregion
        
        #region Public Methods

        public bool CanBePickedUp()
        {
            return playerMover.CanMoveTo(transform.position) && playerInventory.HasSpaceFor(inventoryItem);
        }
        
        public void PickupItem()
        {
            var foundSlot = playerInventory.AddToFirstAvailableSlot(inventoryItem);

            if (foundSlot)
            {
                onPickup?.Invoke();
                OnPickupInventoriable?.Invoke(this);
                
                Destroy(gameObject);
            }
            
            Debug.Log($"Picked up item {inventoryItem.name}");
        }
        
        #endregion
        
        #region Interface Implementations

        public void Setup(InventoryItem item)
        {
            inventoryItem = item;
        }

        public IInventoriable Spawn(Vector3 position)
        {
            return Instantiate(this, position, Quaternion.identity);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public InventoryItem GetItem()
        {
            return inventoryItem;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public UnityEvent<IInventoriable> OnPickupInventoriable { get; } = new();

        #endregion
    }
}