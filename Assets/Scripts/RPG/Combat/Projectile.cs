using UnityEngine;
using Utils;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float aimLocationFraction = .8f;
        [SerializeField] private bool isFollowingTarget = false;
        [SerializeField] private AudioClip[] shootingSounds;
        [SerializeField] private HitEffect hitEffect = null;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifetimeAfterHit = 2f;
        [SerializeField] private float maxLifeTime = 10f;
        
        private Health target;
        private GameObject instigator;
        private float damage;

        #region Unity Callbacks

        private void Start()
        {
            if (shootingSounds.Length <= 0) return;
            
            transform.LookAt(GetAimLocation());

            var shootClip = shootingSounds.GetRandomClip();
            GetComponent<AudioSource>().PlayOneShot(shootClip);
            
            Destroy(gameObject, maxLifeTime);
        }

        private void Update()
        {
            if (!target) return;

            if (isFollowingTarget && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead) return; 
            
            speed = 0;
            target.TakeDamage(instigator, damage);

            if (hitEffect)
            {
                Instantiate(hitEffect.gameObject, transform.position, transform.rotation);
            }

            foreach (var toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifetimeAfterHit);
        }

        #endregion
        
        #region Public Methods
        
        public void SetTarget(Health projectileTarget, GameObject projectileInstigator, float weaponDamage)
        {
            target = projectileTarget;
            damage = weaponDamage;
            instigator = projectileInstigator;
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