using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace RPG.Core
{
    public class SoundPlayer : MonoBehaviour
    {
        [Range(-3f, 3f)]
        [SerializeField] private float pitch = 1f;
        [SerializeField] private AudioClip[] clips;

        public void PlayRandomClip()
        {
            PlayClip(clips.GetRandomClip());
        }

        public void PlayClip(AudioClip audioClip)
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }

    }
}