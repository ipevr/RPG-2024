using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool alreadyTriggered = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (alreadyTriggered || !other.CompareTag("Player")) return;

            alreadyTriggered = true;
            GetComponent<PlayableDirector>().Play();
        }
        
        #region Interface Implementatation

        public JToken CaptureAsJToken()
        {
            return alreadyTriggered;
        }

        public void RestoreFromJToken(JToken state)
        {
            alreadyTriggered = (bool)state;
        }
        
        #endregion
    }
}