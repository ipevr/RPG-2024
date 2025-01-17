using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private float respawnTime = 5f;
        [SerializeField] private UnityEvent onPickup;

        #region Unity Event Functions
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Pickup(other.GetComponent<Fighter>());
        }

        #endregion

        #region Private Methods

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            onPickup.Invoke();
            
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool status)
        {
            GetComponent<Collider>().enabled = status;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(status);
            }
        }
        
        #endregion
        
        #region Interface Implementations

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButton(0))
            {
                Pickup(player.gameObject.GetComponent<Fighter>());
            }
            return true;
        }

        public void HandleStopRaycasting()
        {
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        
        #endregion
    }
}