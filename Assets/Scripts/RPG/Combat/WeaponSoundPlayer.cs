using UnityEngine;
using RPG.Core;
using Utils;

namespace RPG.Combat
{
    public class WeaponSoundPlayer : MonoBehaviour
    {
        private SoundPlayer soundPlayer;

        private void Awake()
        {
            soundPlayer = GetComponent<SoundPlayer>();
        }

        public void PlayWeaponSound(Weapon weapon)
        {
            var clips = weapon.GetWeaponSounds();
            var clip = clips.GetRandomClip();
            soundPlayer.PlayClip(clip);
        }
    }
}