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
            if (Input.GetMouseButton(0))
            {
                pickup.PickupItem();
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