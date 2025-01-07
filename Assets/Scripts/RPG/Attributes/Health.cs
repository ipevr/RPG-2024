using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private static readonly int DieTriggerId = Animator.StringToHash("die");
        private static readonly int DieFastTriggerId = Animator.StringToHash("dieFast");
        
        [SerializeField] private AudioClip[] deathSounds;

        private float healthPoints = -1f;
        private BaseStats baseStats;

        public bool IsDead { get; private set; }

        #region Unity Callbacks

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            if (healthPoints > -1) return;
            
            healthPoints = baseStats.GetStat(Stat.Health);
            Debug.Log($"Health: Setting health points to {healthPoints}.");
        }

        private void OnEnable()
        {
            baseStats.OnLevelUp += HandleLevelUp;
        }

        private void OnDisable()
        {
            baseStats.OnLevelUp -= HandleLevelUp;
        }

        #endregion
        
        #region Public Methods

        public void TakeDamage(GameObject instigator, float amount)
        {
            if (IsDead) return;
            
            healthPoints = Mathf.Max(healthPoints - amount, 0);

            if (healthPoints > 0) return;
            
            Die(DieTriggerId);
            GainExperience(instigator);
        }

        public float GetPercentage()
        {
            return (healthPoints / baseStats.GetStat(Stat.Health)) * 100f;
        }

        #endregion
        
        #region Private Methods

        private void HandleLevelUp()
        {
            Debug.Log("Health notified level up");
            healthPoints = baseStats.GetStat(Stat.Health);
            Debug.Log($"Setting health to {healthPoints}");
        }

        private void Die(int dieTriggerId, bool silent = false)
        {
            IsDead = true;
            GetComponent<Animator>().SetTrigger(dieTriggerId);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
            if (silent) return;
            
            GetComponent<AudioSource>().PlayOneShot(deathSounds.GetRandomClip());
        }

        private void GainExperience(GameObject instigator)
        {
            var experienceComponent = instigator.GetComponent<Experience>(); 
            if (!experienceComponent) return;
            
            var experienceAmount = baseStats.GetStat(Stat.ExperienceReward);
            experienceComponent.GainExperience(experienceAmount);
        }

        #endregion
        
        #region Interface Implementations

        public JToken CaptureAsJToken()
        {
            return healthPoints;
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                Die(DieFastTriggerId, true);
            }
        }

        #endregion
    }
}