using RPG.Control;
using UnityEngine;

namespace RPG.Pickups
{
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup pickup;

        #region Unity Event Functions
        
        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        #endregion
        
        #region Interface Implementations
        
        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }

            // Todo: Nicer solution: Shift-Click item --> Desired amount can be adjusted in ui popup 
            if (Input.GetMouseButtonDown(1))
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