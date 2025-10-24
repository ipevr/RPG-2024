using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public float ExperiencePoints { get; private set; }
        
        public UnityEvent onExperienceGained;

        #region Public Methods
        
        public void GainExperience(float experience)
        {
            var multiplicator = ModifierAggregator.GetPercentageFraction(gameObject, Stat.ExperienceReward);
            var additionalExp = ModifierAggregator.GetAdditive(gameObject, Stat.ExperienceReward);
            ExperiencePoints += (experience + additionalExp) * multiplicator;
            onExperienceGained?.Invoke();
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