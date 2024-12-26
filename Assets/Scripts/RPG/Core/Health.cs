using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        private static readonly int DieTriggerId = Animator.StringToHash("die");
        
        [SerializeField] private float healthPoints = 100f;

        public bool IsDead { get; private set; }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            
            healthPoints = Mathf.Max(healthPoints - amount, 0);

            if (healthPoints == 0)
            {
                Die();
            }
            
            Debug.Log(healthPoints);
        }

        private void Die()
        {
            IsDead = true;
            GetComponent<Animator>().SetTrigger(DieTriggerId);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}