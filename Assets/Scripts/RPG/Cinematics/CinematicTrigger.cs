using System;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

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