using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask ignoredLayers;
        [SerializeField] private float normalSpeedFraction = 1f;
        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            Debug.Log("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            foreach (var hit in Physics.RaycastAll(GetMouseRay(), Mathf.Infinity, ~ignoredLayers))
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (!target) continue;
                
                if (!target || !GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit, Mathf.Infinity, ~ignoredLayers);
            if (!hasHit) return false;
            
            if (Input.GetMouseButton(0))
            {
                GetComponent<Mover>().StartMoveAction(hit.point, normalSpeedFraction);
            }

            return true;
        }

        private Ray GetMouseRay()
        {
            var mainCamera = Camera.main;
            if (!mainCamera)
            {
                throw new MissingReferenceException("No main camera found");
            }
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

    }
}
