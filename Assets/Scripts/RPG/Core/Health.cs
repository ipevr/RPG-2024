using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        private static readonly int DieTriggerId = Animator.StringToHash("die");
        private static readonly int DieFastTriggerId = Animator.StringToHash("dieFast");
        
        [SerializeField] private float healthPoints = 100f;

        public bool IsDead { get; private set; }

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

        private void Die(int dieTriggerId)
        {
            IsDead = true;
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

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
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