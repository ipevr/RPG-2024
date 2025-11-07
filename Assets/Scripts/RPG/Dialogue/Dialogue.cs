using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes = new ();
        
        private readonly Dictionary<string, DialogueNode> nodeLookup = new();

        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }

            OnValidate();
        }

        private void OnValidate()
        {
            BuildNodeLookUp();
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public void CreateNode(DialogueNode parentNode)
        {
            var newNode = new DialogueNode();
            nodes.Add(newNode);
            parentNode.AddChild(newNode.UniqueId);
            newNode.SetPosition(new Vector2(parentNode.Rect.x + 250, parentNode.Rect.y));
            OnValidate();
        }
        
        public void DeleteNode(DialogueNode nodeToDelete)
        {
            foreach (var node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.UniqueId);
            }
            nodes.Remove(nodeToDelete);
            OnValidate();
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (var childId in parentNode.Children)
            {
                if (nodeLookup.TryGetValue(childId, out var node))
                {
                    yield return node;
                }
            }
        }

        private void BuildNodeLookUp()
        {
            nodeLookup.Clear();

            foreach (var node in GetAllNodes())
            {
                nodeLookup.Add(node.UniqueId, node);
            }
        }

    }
}