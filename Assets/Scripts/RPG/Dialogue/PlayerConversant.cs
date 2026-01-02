using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private string playerName;
        
        private Dialogue currentDialogue;
        private DialogueNode currentNode;
        private bool isChoosing;
        private AIConversant currentConversant;
        private string currentConversantName;

        public DialogueNode CurrentNode => currentNode;
        public bool IsChoosing => isChoosing;
        
        public UnityAction OnConversationUpdated;
        
        public static PlayerConversant GetPlayerConversant()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerConversant>();
        }
        
        public void StartDialogue(AIConversant newConversant)
        {
            currentDialogue = newConversant.Dialogue;
            currentConversant = newConversant;
            currentConversantName = newConversant.Name;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            isChoosing = currentNode.IsPlayerSpeaking;
            
            OnConversationUpdated?.Invoke();
        }
        
        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            OnConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public string GetSpeakerName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            
            return currentConversant ? currentConversantName : "";
        }
        
        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }
        
        public void SelectChoice(DialogueNode selectedChoice)
        {
            currentNode = selectedChoice;
            TriggerEnterAction();
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
                TriggerExitAction();
                OnConversationUpdated?.Invoke();
                return;
            }
            
            var numAIResponses = currentDialogue.GetAIChildren(currentNode).Count();
            
            if (numAIResponses > 0)
            {
                var aiResponses = currentDialogue.GetAIChildren(currentNode).ToArray();
                TriggerExitAction();
                currentNode = aiResponses[UnityEngine.Random.Range(0, aiResponses.Length)];
                TriggerEnterAction();
                isChoosing = false;
            }

            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            return currentDialogue && currentNode &&
                   currentDialogue.GetAllChildren(currentNode).Any();
        }

        private void TriggerEnterAction()
        {
            TriggerAction(currentNode.OnEnterAction);
        }

        private void TriggerExitAction()
        {
            TriggerAction(currentNode.OnExitAction);
        }

        private void TriggerAction(DialogueAction action)
        {
            if (!currentNode || action == DialogueAction.None) return;
            if (!currentConversant) return;

            var triggers = currentConversant.GetComponents<DialogueTrigger>();
            
            foreach (var trigger in triggers)
            {
                trigger.Trigger(action);
            }
        }

    }
}