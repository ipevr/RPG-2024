using System;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Control;

namespace RPG.Dialogues
{
    public class AIConversant : MonoBehaviour, IRaycastable, IConversant
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private InputActionReference interactAction;
        
        public Dialogue GetDialogue()
        {
            return dialogue;
        }

        public string GetName()
        {
            return name;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (!enabled) return false;
            
            if (interactAction && interactAction.action.triggered)
            {
                StartDialogue();
            }

            return true;
        }
        
        public void StartDialogue()
        {
            PlayerConversant.GetPlayerConversant().StartDialogue(this);
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

    }
}