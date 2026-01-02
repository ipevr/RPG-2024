using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Control;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private string conversantName;
        
        public Dialogue Dialogue => dialogue;
        public string Name => conversantName;

        public bool HandleRaycast(PlayerController player)
        {
            if (!enabled) return false;
            if (!dialogue) return false;
            
            if (interactAction && interactAction.action.triggered)
            {
                PlayerConversant.GetPlayerConversant().StartDialogue(this);
            }

            return true;
        }
        
        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }
    }
}