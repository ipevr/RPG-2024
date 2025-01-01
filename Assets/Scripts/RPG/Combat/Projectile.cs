using UnityEngine;
using RPG.Core;
using UnityEngine.Serialization;

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
        

        private const float MaxDistance = 100;

        private Vector3 startPosition;
        private Health target;
        private float damage;

        #region Unity Callbacks

        private void Start()
        {
            startPosition = transform.position;
            if (shootingSounds.Length <= 0) return;
            
            var shootClip = GetClip(shootingSounds);
            GetComponent<AudioSource>().PlayOneShot(shootClip);
        }

        private void Update()
        {
            if (!target) return;

            if (isFollowingTarget && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
            
            if (IsMaxDistanceExceeded())
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead) return; 
            
            target.TakeDamage(damage);

            if (hitEffect)
            {
                Instantiate(hitEffect.gameObject, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }

        #endregion
        
        #region Public Methods
        
        public void SetTarget(Health projectileTarget, float weaponDamage)
        {
            target = projectileTarget;
            damage = weaponDamage;
            transform.LookAt(GetAimLocation());
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
        
        private bool IsMaxDistanceExceeded()
        {
            return Vector3.Distance(transform.position, startPosition) > MaxDistance;
        }

        private AudioClip GetClip(AudioClip[] clips)
        {
            var clipIndex = Random.Range(0, clips.Length);
            return clips[clipIndex];
        }

        #endregion
    }
}