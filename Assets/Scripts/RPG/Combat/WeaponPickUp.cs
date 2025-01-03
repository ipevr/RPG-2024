using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private PickupSound pickupSound;
        [SerializeField] private float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            other.gameObject.GetComponent<Fighter>().EquipWeapon(weapon);
            
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
    }
}