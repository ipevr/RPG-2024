using System;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float aimLocationFraction = .8f;

        private Health target;
        private float damage;

        #region Unity Callbacks

        private void Update()
        {
            if (!target) return;
            
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            
            target.TakeDamage(damage);
            
            Destroy(gameObject);
        }

        #endregion
        
        #region Public Methods
        
        public void SetTarget(Health projectileTarget, float weaponDamage)
        {
            target = projectileTarget;
            damage = weaponDamage;
        }
        
        #endregion
        
        #region Private Methods

        private Vector3 GetAimLocation()
        {
            var capsuleCollider = target.GetComponent<CapsuleCollider>();
            if (!capsuleCollider)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * (capsuleCollider.height * aimLocationFraction);
        }
        
        #endregion
    }
}