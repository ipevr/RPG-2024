using UnityEngine;
using Utils;

namespace RPG.Core
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;

        public void PlayRandomClip()
        {
            PlayClip(clips.GetRandomClip());
        }

        public void PlayClip(AudioClip audioClip)
        {
            GetComponent<AudioSource>().PlayOneShot(audioClip);
        }

    }
}