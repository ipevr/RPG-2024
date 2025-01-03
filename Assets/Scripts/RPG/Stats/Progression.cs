using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;
        
        [Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
            public float[] damage;
        }

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (var progressionCharacterClass in characterClasses)
            {
                if (progressionCharacterClass.characterClass.Equals(characterClass))
                {
                    return progressionCharacterClass.health[level - 1];
                }
            }

            return -1;
        }
    }
}