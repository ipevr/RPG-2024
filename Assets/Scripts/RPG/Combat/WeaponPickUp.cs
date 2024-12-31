using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            other.gameObject.GetComponent<Fighter>().EquipWeapon(weapon);
            
            Destroy(gameObject);
        }
    }
}