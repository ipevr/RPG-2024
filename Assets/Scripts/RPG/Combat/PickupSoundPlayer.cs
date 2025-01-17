using UnityEngine;
using Utils;
using RPG.Core;

namespace RPG.Combat
{
    public class PickupSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] pickupSounds;
        [SerializeField] private SoundPlayer soundPlayerPrefab;

        public void PlayPickupSound()
        {
            var soundPlayer = Instantiate(soundPlayerPrefab, transform.position, Quaternion.identity);

            var clip = pickupSounds.GetRandomClip();
            soundPlayer.PlayClip(clip);
            
            Destroy(soundPlayer.gameObject, clip.length);
        }

    }
}