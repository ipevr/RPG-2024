using UnityEngine;
using Utils;
using RPG.Attributes;

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
        [SerializeField] private float damage = 5f;

        private const string WeaponTag = "Weapon";

        public float Range => range;

        public float Damage => damage;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            
            if (equippedPrefab != null)
            {
                var weapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
                weapon.tag = WeaponTag;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
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

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            var projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage);
        }

    }
}