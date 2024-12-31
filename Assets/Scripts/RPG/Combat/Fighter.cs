using UnityEngine;
using RPG.Core;
using RPG.Movement;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private static readonly int AttackTriggerId = Animator.StringToHash("attack");
        private static readonly int StopAttackId = Animator.StringToHash("stopAttack");

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        
        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon;
        
        #region Unity Callbacks

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

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

        #endregion
        
        #region Public Methods
        
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        #endregion

        #region Private Methods
        
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
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.Range;
        }

        #endregion

        #region Interface Implementations
        
        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        #endregion
        
        #region Animation Events
        
        private void Hit()
        {
            if (!target) return;
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.Damage);
            }
        }
        
        private void Shoot()
        {
            Hit();
        }

        #endregion

        
    }
}