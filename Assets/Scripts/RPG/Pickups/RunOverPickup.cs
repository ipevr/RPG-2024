using UnityEngine;

namespace RPG.Pickups
{
    public class RunOverPickup : MonoBehaviour
    {
        private Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            pickup.PickupItem();
        }

    }
}