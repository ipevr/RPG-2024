using UnityEngine;
using RPG.Core;

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

        public float Range => range;

        public float Damage => damage;

        public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab == null) return null;
            var equippedWeapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            return equippedWeapon;
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