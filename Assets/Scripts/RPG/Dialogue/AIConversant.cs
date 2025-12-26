using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Control;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private InputActionReference interactAction;
        
        public bool HandleRaycast(PlayerController player)
        {
            if (!dialogue) return false;
            
            if (interactAction && interactAction.action.triggered)
            {
                player.GetComponent<PlayerConversant>().StartDialogue(dialogue);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }
    }
}