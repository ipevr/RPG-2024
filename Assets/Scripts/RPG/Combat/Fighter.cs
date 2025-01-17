using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using RPG.Movement;
using RPG.Saving;
using RPG.Attributes;
using RPG.Core;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        private static readonly int AttackTriggerId = Animator.StringToHash("attack");
        private static readonly int StopAttackId = Animator.StringToHash("stopAttack");

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private UnityEvent<Weapon> onHit;
        
        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private LazyValue<Weapon> currentWeapon;
        
        #region Unity Event Functions

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
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
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        public Health GetTarget()
        {
            return target;
        }

        #endregion

        #region Private Methods

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }
        
        private void AttachWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
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
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.value.Range;
        }

        #endregion

        #region Animation Events
        
        private void Hit()
        {
            if (!target) return;
            
            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            currentWeapon.value.MakeDamage(rightHandTransform, leftHandTransform, target, gameObject, damage);
            onHit.Invoke(currentWeapon.value);
        }
        
        private void Shoot()
        {
            Hit();
        }

        #endregion

        #region Interface Implementations
        
        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        public JToken CaptureAsJToken()
        {
            return currentWeapon.value.name;
        }

        public void RestoreFromJToken(JToken state)
        {
            var weaponName = (string)state;
            var weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.WeaponPercentageBonus;
            }
        }

        #endregion
    }
}