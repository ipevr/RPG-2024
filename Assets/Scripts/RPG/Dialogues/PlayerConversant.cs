using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using UnityEditor.TerrainTools;

namespace RPG.Dialogues
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private string playerName;
        
        private Dialogue currentDialogue;
        private DialogueNode currentNode;
        private bool isChoosing;
        private IConversant currentConversant;
        private string currentConversantName;

        public DialogueNode CurrentNode => currentNode;
        public bool IsChoosing => isChoosing;
        
        public UnityAction OnConversationUpdated;
        
        public static PlayerConversant GetPlayerConversant()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerConversant>();
        }
        
        public void StartDialogue(IConversant newConversant)
        {
            currentDialogue = newConversant.GetDialogue();
            currentConversant = newConversant;
            currentConversantName = newConversant.GetName();
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
            
            return currentConversant != null ? currentConversantName : "";
        }
        
        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
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

            var numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();

            if (numPlayerResponses > 0)
            {
                isChoosing = true; 
                TriggerExitAction();
                OnConversationUpdated?.Invoke();
                return;
            }
            
            var numAIResponses = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).Count();
            
            if (numAIResponses > 0)
            {
                var aiResponses = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
                foreach (var response in aiResponses)
                {
                    Debug.Log($"A possible AI Response: {response.Text}");
                }
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
                   FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Any();
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNodes)
        {
            foreach (var node in inputNodes)
            {
                if (node.CheckConditions(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
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
            if (currentConversant == null) return;

            var triggers = currentConversant.GetGameObject().GetComponents<DialogueTrigger>();
            
            foreach (var trigger in triggers)
            {
                trigger.Trigger(action);
            }
        }

    }
}