using RPG.Inventory;
using RPG.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Pickups
{
    public class Pickup : MonoBehaviour, IInventoriable
    {
        [SerializeField] private UnityEvent<int> onPickup;
        private int currentAmount;

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

        public UnityEvent<IInventoriable> OnPickupInventoriable { get; } = new();

        #region Public Methods

        public bool CanBePickedUp()
        {
            return playerMover.CanMoveTo(transform.position) && playerInventory.HasSpaceFor(inventoryItem);
        }

        public void PickupItem(int amount)
        {
            amount = Mathf.Min(amount, currentAmount);

            var amountPickedUp = playerInventory.AddToFirstAvailableSlot(inventoryItem, amount);

            currentAmount -= amountPickedUp;

            if (amountPickedUp > 0)
            {
                onPickup?.Invoke(amountPickedUp);
                OnPickupInventoriable?.Invoke(this);
            }

            if (currentAmount <= 0) Destroy();

            Debug.Log($"Picked up item {inventoryItem.name} with amount {amountPickedUp}");
        }

        public void PickupItem()
        {
            PickupItem(currentAmount);
        }

        #endregion

        #region Interface Implementations

        public void Setup(InventoryItem item, int itemAmount)
        {
            inventoryItem = item;
            currentAmount = itemAmount;
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

        public int GetAmount()
        {
            return currentAmount;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        #endregion
    }
}