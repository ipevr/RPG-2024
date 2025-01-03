using UnityEngine;
using Utils;

namespace RPG.Combat
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip[] hitClips = null;

        private void Start()
        {
            var clip = hitClips.GetRandomClip();
            GetComponent<AudioSource>().PlayOneShot(clip);

            var particleEffect = GetComponent<ParticleSystem>(); 
            particleEffect.Play();
            
            Destroy(gameObject, Mathf.Max(clip.length, particleEffect.main.duration));
        }
    }
}
