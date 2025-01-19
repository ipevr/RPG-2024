using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig weaponConfig;
        [SerializeField] private float healthToRestore = 0f;
        [SerializeField] private float respawnTime = 5f;
        [SerializeField] private UnityEvent onPickup;

        #region Unity Event Functions
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Pickup(other.gameObject);
        }

        #endregion

        #region Private Methods

        private void Pickup(GameObject subject)
        {
            if (weaponConfig)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weaponConfig);
            }
            
            subject.GetComponent<Health>().Heal(healthToRestore);
            
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
                Pickup(player.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        
        #endregion
    }
}