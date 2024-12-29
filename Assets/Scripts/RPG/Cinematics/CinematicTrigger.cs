using System;
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

        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
        }
        
        #endregion
    }
}