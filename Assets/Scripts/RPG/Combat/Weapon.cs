using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapons/Create new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private AudioClip[] weaponSounds;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private float range = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float weaponPercentageBonus = 0;

        private const string WeaponName = "Weapon";
        
        private GameObject weapon = null;

        public float Range => range;
        public float WeaponDamage => weaponDamage;
        public float WeaponPercentageBonus => weaponPercentageBonus;
        
        #region Public Methods

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            
            if (equippedPrefab)
            {
                weapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
                weapon.name = WeaponName;
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

        public AudioClip[] GetWeaponSounds()
        {
            return weaponSounds;
        }

        public void MakeDamage(Transform rightHand, Transform leftHand, Health target, GameObject instigator,
            float damage)
        {
            if (projectile != null)
            {
                LaunchProjectile(rightHand, leftHand, target, instigator, damage);
            }
            else
            {
                target.TakeDamage(instigator, damage);
            }
        }

        #endregion

        #region Private Methods
        
        private void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float damage)
        {
            var projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, damage);
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(WeaponName);
            if (!oldWeapon)
            {
                oldWeapon = leftHand.Find(WeaponName);
            }

            if (!oldWeapon) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        #endregion
    }
}