using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        private Dialogue currentDialogue;
        private DialogueNode currentNode;

        public DialogueNode CurrentNode => currentNode;

        private bool isChoosing; // explicit UI state: are we showing player choices?

        public bool IsChoosing => isChoosing;
        
        public UnityAction OnConversationUpdated;
        
        public static PlayerConversant GetPlayerConversant()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerConversant>();
        }
        
        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            isChoosing = currentNode.IsPlayerSpeaking;
            
            OnConversationUpdated?.Invoke();
        }
        
        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            OnConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }
        
        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }
        
        public void SelectChoice(DialogueNode selectedChoice)
        {
            currentNode = selectedChoice;
            isChoosing = false;
            Next();
        }


        public void Next()
        {
            if (!HasNext())
            {
                Quit();
                return;
            }

            var numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();

            if (numPlayerResponses > 0)
            {
                isChoosing = true; 
                OnConversationUpdated?.Invoke();
                return;
            }
            
            var numAIResponses = currentDialogue.GetAIChildren(currentNode).Count();
            
            if (numAIResponses > 0)
            {
                var aiResponses = currentDialogue.GetAIChildren(currentNode).ToArray();
                currentNode = aiResponses[UnityEngine.Random.Range(0, aiResponses.Length)];
                isChoosing = false;
            }

            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            return currentDialogue != null && currentNode != null &&
                   currentDialogue.GetAllChildren(currentNode).Any();
        }

    }
}