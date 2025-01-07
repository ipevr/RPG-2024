using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacter[] characterClasses;
        
        [Serializable]
        private class ProgressionCharacter
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }
        
        [Serializable]
        private class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            var levels = lookupTable[characterClass][stat];
            return levels.Length < level ? levels[^1] : levels[level - 1];
        }

        public int GetNumberOfLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            return lookupTable[characterClass][stat].Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            
            foreach (var progressionCharacter in characterClasses)
            {
                lookupTable.Add(progressionCharacter.characterClass, new Dictionary<Stat, float[]>());
                
                foreach (var progressionStat in progressionCharacter.stats)
                {
                    lookupTable[progressionCharacter.characterClass].Add(progressionStat.stat, progressionStat.levels);
                }
            }
        }
    }
}
