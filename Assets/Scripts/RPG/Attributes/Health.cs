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
        
        [SerializeField] private float healthPoints = 100f;
        [SerializeField] private AudioClip[] deathSounds;

        public bool IsDead { get; private set; }

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            
            healthPoints = Mathf.Max(healthPoints - amount, 0);

            if (healthPoints <= 0)
            {
                DieNormal();
            }
            
            Debug.Log($"Health of {gameObject.name}: {healthPoints}");
        }

        public float GetPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetHealth()) * 100f;
        }

        private void Die(int dieTriggerId)
        {
            IsDead = true;
            GetComponent<AudioSource>().PlayOneShot(deathSounds.GetRandomClip());
            GetComponent<Animator>().SetTrigger(dieTriggerId);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
        }

        private void DieNormal()
        {
            Die(DieTriggerId);
        }

        private void DieFast()
        {
            Die(DieFastTriggerId);
        }
        
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
                DieFast();
            }
        }

        #endregion
    }
}