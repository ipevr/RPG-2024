using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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

        [SerializeField] private float levelUpRegenerationPercentage = 100;
        [SerializeField] private UnityEvent<float> onTakeDamage;
        [SerializeField] private UnityEvent onDie; 

        private LazyValue<float> healthPoints;
        private BaseStats baseStats;

        public bool IsDead { get; private set; }

        #region Unity Event Functions

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealthPoints);
        }

        private void Start()
        {
            healthPoints.ForceInit();
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

        public void TakeDamage(GameObject instigator, float damage)
        {
            Debug.Log($"{gameObject.name} takes damage: {damage}.");
            if (IsDead) return;
            
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (healthPoints.value > 0)
            {
                onTakeDamage.Invoke(damage);
                return;
            };
            
            onDie.Invoke();
            Die(DieTriggerId);
            GainExperience(instigator);
        }

        public float GetHealthPoints() => healthPoints.value;
        
        public float GetMaxHealthPoints() => baseStats.GetStat(Stat.Health);

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        public float GetPercentage()
        {
            return GetFraction() * 100f;
        }

        public float GetFraction()
        {
            return healthPoints.value / baseStats.GetStat(Stat.Health);
        }
        
        #endregion
        
        #region Private Methods

        private float GetInitialHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void HandleLevelUp()
        {
            var regenHealthPoints = baseStats.GetStat(Stat.Health) * (levelUpRegenerationPercentage / 100f); 
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        private void Die(int dieTriggerId)
        {
            IsDead = true;
            GetComponent<Animator>().SetTrigger(dieTriggerId);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
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
            return healthPoints.value;
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                Die(DieFastTriggerId);
            }
        }

        #endregion
    }
}