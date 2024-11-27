using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace RPG.Combat
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
            // GetComponent<Collider>().enabled = false;
            // GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}