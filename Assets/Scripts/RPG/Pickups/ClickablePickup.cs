using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Control;

namespace RPG.Pickups
{
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private InputActionReference pickupAllAction; 
        [SerializeField] private InputActionReference pickupOneAction; 
        private Pickup pickup;

        #region Unity Event Functions
        
        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        #endregion
        
        #region Interface Implementations
        
        public bool HandleRaycast(PlayerController player)
        {
            if (pickupAllAction && pickupAllAction.action.triggered)
            {
                pickup.PickupItem();
            }

            // Todo: Nicer solution: Shift-Click item --> Desired amount can be adjusted in ui popup 
            if (pickupOneAction && pickupOneAction.action.triggered)
            {
                pickup.PickupItem(1);
            }
            return true;

        }

        public CursorType GetCursorType()
        {
            return pickup.CanBePickedUp() ? CursorType.Pickup : CursorType.None;
        }
        
        #endregion
    }
}