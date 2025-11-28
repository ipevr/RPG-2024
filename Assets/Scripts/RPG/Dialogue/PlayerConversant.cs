using System;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue currentDialogue;
        
        private DialogueNode currentNode;

        private void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
        }

        public static PlayerConversant GetPlayerConversant()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<PlayerConversant>();
        }

        public string GetText()
        {
            return !currentNode ? "" : currentNode.Text;
        }

        public bool MoveToNextNode()
        {
            if (!currentNode)
            {
                return false;
            }
            if (currentNode.Children == null)
            {
                return false;
            }
            var nodeChildren = currentDialogue.GetAllChildren(currentNode).ToArray();
            if (nodeChildren.Length > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, nodeChildren.Length);
                currentNode = nodeChildren[randomIndex];
                return true;
            }
            return false;
        }

    }
}