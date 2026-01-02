using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        [SerializeField] private InputActionReference attackAction;

        public bool HandleRaycast(PlayerController player)
        {
            if (!enabled) return false;
            
            if (!player.GetComponent<Fighter>().CanAttack(gameObject)) return false;
            
            if (attackAction && attackAction.action.triggered)
            {
                player.GetComponent<Fighter>().Attack(gameObject);
            }
            
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

    }
}
