using System.Collections;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private PickupSound pickupSound;
        [SerializeField] private float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Pickup(other.GetComponent<Fighter>());
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            
            if (pickupSound)
            {
                Instantiate(pickupSound, transform.position, Quaternion.identity);
            }
            
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0))
            {
                Pickup(callingController.gameObject.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}