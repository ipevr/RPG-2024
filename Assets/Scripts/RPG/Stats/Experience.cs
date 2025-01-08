using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action OnExperienceGained;
        
        public float ExperiencePoints { get; private set; }

        #region Public Methods
        
        public void GainExperience(float experience)
        {
            ExperiencePoints += experience;
            OnExperienceGained?.Invoke();
        }

        #endregion
        
        #region Interface Implementations

        public JToken CaptureAsJToken()
        {
            return ExperiencePoints;
        }

        public void RestoreFromJToken(JToken state)
        {
            ExperiencePoints = (float) state;
        }

        #endregion

    }
}