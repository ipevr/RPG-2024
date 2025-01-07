using UnityEngine;

namespace RPG.Core
{
    public class AudioController : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float globalVolume = 1;

        private void Start()
        {
            AudioListener.volume = globalVolume; 
        }
    }
}