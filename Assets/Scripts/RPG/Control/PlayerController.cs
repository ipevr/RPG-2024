using RPG.Combat;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            Debug.Log("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            foreach (var hit in Physics.RaycastAll(GetMouseRay()))
            {
                var target = hit.transform.GetComponent<CombatTarget>(); 
                if (!GetComponent<Fighter>().CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;
            
            if (Input.GetMouseButton(0))
            {
                GetComponent<Mover>().StartMoveAction(hit.point);
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
