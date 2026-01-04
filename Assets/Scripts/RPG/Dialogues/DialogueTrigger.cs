using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogues
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueAction dialogueAction;
        [SerializeField] private UnityEvent onTrigger;

        public void Trigger(DialogueAction action)
        {
            if (action != dialogueAction) return;
            
            onTrigger.Invoke();
        }
    }
}