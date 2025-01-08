using System;
using UnityEngine;

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

        private int currentLevel = 0;
        private Experience experience;
        
        public event Action OnLevelUp;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (experience)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience)
            {
                experience.OnExperienceGained -= UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private void UpdateLevel()
        {
            var level = CalculateLevel();

            if (level > currentLevel)
            {
                ProcessLevelUp(level);
            }
        }

        private void ProcessLevelUp(int level)
        {
            currentLevel = level;
            OnLevelUp?.Invoke();
            Instantiate(levelUpEffect, levelUpEffectSpawnPoint);
            GetComponent<AudioSource>().PlayOneShot(levelUpSound);
        }

        private int CalculateLevel()
        {
            Debug.Log("Calculating level");
            if (!experience) return startingLevel;
            
            var penultimateLevel = progression.GetNumberOfLevels(Stat.ExperienceToLevelUp, characterClass);

            for (var level = startingLevel; level <= penultimateLevel; level++)
            {
                var experienceToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (experience.ExperiencePoints < experienceToLevelUp) return level;
            }

            return penultimateLevel + 1;
        }
    }
}