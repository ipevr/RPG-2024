using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private static readonly int AttackTriggerId = Animator.StringToHash("attack");
        private static readonly int StopAttackId = Animator.StringToHash("stopAttack");

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 10f;
        
        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target || target.IsDead) return;

            if (!IsInRange())
            {
                MoveToTarget();
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget) return false;
            
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest && !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();

                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger(StopAttackId);
            GetComponent<Animator>().SetTrigger(AttackTriggerId);
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(AttackTriggerId);
            GetComponent<Animator>().SetTrigger(StopAttackId);
        }

        private void MoveToTarget()
        {
            GetComponent<Mover>().MoveTo(target.transform.position, 1f);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        // Animation Event
        private void Hit()
        {
            if (!target) return;
            target.TakeDamage(weaponDamage);
        }
    }
}