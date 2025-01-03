using UnityEngine;

namespace RPG.Combat
{
    public class PickupSound : MonoBehaviour
    {
        [SerializeField] private AudioClip pickupClip;

        public void Start()
        {
            GetComponent<AudioSource>().PlayOneShot(pickupClip);
            
            Destroy(gameObject, pickupClip.length);
        }
        
        
    }
}