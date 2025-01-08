using UnityEngine;
using Utils;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapons/Create new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private float range = 2f;
        [SerializeField] private float weaponDamage = 5f;

        private const string WeaponTag = "Weapon";

        public float Range => range;

        #region Public Methods

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            
            if (equippedPrefab)
            {
                var weapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
                weapon.tag = WeaponTag;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public void MakeDamage(Transform rightHand, Transform leftHand, Health target, GameObject instigator,
            float baseDamage)
        {
            var combinedDamage = baseDamage + weaponDamage;
            if (projectile != null)
            {
                LaunchProjectile(rightHand, leftHand, target, instigator, combinedDamage);
            }
            else
            {
                target.TakeDamage(instigator, combinedDamage);
            }
        }

        #endregion

        #region Private Methods
        
        private void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float combinedDamage)
        {
            var projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, combinedDamage);
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.FindByTag(WeaponTag);
            if (!oldWeapon)
            {
                oldWeapon = leftHand.FindByTag(WeaponTag);
            }

            if (!oldWeapon) return;

            oldWeapon.tag = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        #endregion
    }
}