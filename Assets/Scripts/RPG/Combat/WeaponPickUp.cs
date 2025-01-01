using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private AudioClip pickupClip;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            other.gameObject.GetComponent<Fighter>().EquipWeapon(weapon);
            
            GetComponent<AudioSource>().PlayOneShot(pickupClip);
            
            DisableChildren();
            Destroy(gameObject, pickupClip.length);
        }
        
        private void DisableChildren()
        {
            var children = transform.childCount;
            for (var i = 0; i < children; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}