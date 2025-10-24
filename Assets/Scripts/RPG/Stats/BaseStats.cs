using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private ParticleSystem levelUpEffect;
        [SerializeField] private Transform levelUpEffectSpawnPoint;
        [SerializeField] private AudioClip levelUpSound;
        [SerializeField] private bool shouldUseModifiers = false;

        private LazyValue<int> currentLevel;
        private Experience experience;
        
        public UnityEvent onLevelUp;

        #region Unity Event Functions

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience)
            {
                experience.onExperienceGained.AddListener(UpdateLevel); 
            }
        }

        private void OnDisable()
        {
            if (experience)
            {
                experience.onExperienceGained.RemoveListener(UpdateLevel);
            }
        }

        #endregion
        
        #region Public Methods
        
        public float GetStat(Stat stat)
        {
            if (shouldUseModifiers)
            {
                var modifierAddPoints = ModifierAggregator.GetAdditive(gameObject, stat);
                var modifierPercentageFraction = ModifierAggregator.GetPercentageFraction(gameObject, stat);
                return (GetBaseStat(stat) + modifierAddPoints) * modifierPercentageFraction;
            }
            
            return GetBaseStat(stat);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        #endregion

        #region Private Methods

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private void UpdateLevel()
        {
            var level = CalculateLevel();

            if (level > currentLevel.value)
            {
                ProcessLevelUp(level);
            }
        }

        private void ProcessLevelUp(int level)
        {
            currentLevel.value = level;
            onLevelUp?.Invoke();
            Instantiate(levelUpEffect, levelUpEffectSpawnPoint);
            GetComponent<AudioSource>().PlayOneShot(levelUpSound);
        }

        private int CalculateLevel()
        {
            if (!experience) return startingLevel;
            
            var penultimateLevel = progression.GetNumberOfLevels(Stat.ExperienceToLevelUp, characterClass);

            for (var level = startingLevel; level <= penultimateLevel; level++)
            {
                var experienceToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (experience.ExperiencePoints < experienceToLevelUp) return level;
            }

            return penultimateLevel + 1;
        }
        
        #endregion
    }
}