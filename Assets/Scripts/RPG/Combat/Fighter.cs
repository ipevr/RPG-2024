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
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        private static readonly int AttackTriggerId = Animator.StringToHash("attack");
        private static readonly int StopAttackId = Animator.StringToHash("stopAttack");

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeaponConfig = null;
        
        private Health target;
        private Mover mover;
        private float timeSinceLastAttack = Mathf.Infinity;
        private WeaponConfig currentWeaponConfig;
        
        #region Unity Event Functions

        private void Awake()
        {
            currentWeaponConfig = defaultWeaponConfig;
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            if (currentWeaponConfig)
            {
                AttachWeapon(currentWeaponConfig);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target || target.IsDead) return;

            if (!IsInRange(target.transform))
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
            
            if (!mover.CanMoveTo(combatTarget.transform.position) && !IsInRange(combatTarget.transform)) return false;
            
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest && !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            AttachWeapon(weaponConfig);
        }

        public Health GetTarget()
        {
            return target;
        }

        #endregion

        #region Private Methods

        private void AttachWeapon(WeaponConfig weaponConfig)
        {
            weaponConfig.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
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
            mover.MoveTo(target.transform.position, 1f);
        }

        private bool IsInRange(Transform transformTarget)
        {
            return Vector3.Distance(transform.position, transformTarget.transform.position) <= currentWeaponConfig.Range;
        }

        #endregion

        #region Animation Events
        
        private void Hit()
        {
            if (!target) return;
            
            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            currentWeaponConfig.MakeDamage(rightHandTransform, leftHandTransform, target, gameObject, damage);
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
            return currentWeaponConfig.name;
        }

        public void RestoreFromJToken(JToken state)
        {
            var weaponName = (string)state;
            var weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.WeaponPercentageBonus;
            }
        }

        #endregion
    }
}