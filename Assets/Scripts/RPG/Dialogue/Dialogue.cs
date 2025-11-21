using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes = new ();
        
        private readonly Dictionary<string, DialogueNode> nodeLookup = new();
        
        public UnityEvent onUndoRedoPerformed = new();

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (nodes.Count == 0)
            {
                var newNode = MakeNode(null);
                AddNode(newNode);
            }

            BuildNodeLookUp();
        }
        
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
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
                if (node)
                {
                    nodeLookup.Add(node.name, node);
                }
            }
        }
        
#if UNITY_EDITOR        
        private void OnEnable()
        {
            Undo.undoRedoPerformed += HandleUndoRedo;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= HandleUndoRedo;
        }

        public void CreateNode(DialogueNode parentNode)
        {
            var newNode = MakeNode(parentNode);
            newNode.onUndoRedoPerformed.AddListener(HandleUndoRedo);
            
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);

            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Delete Dialogue Node");
            nodeToDelete.onUndoRedoPerformed.RemoveListener(HandleUndoRedo);
            nodes.Remove(nodeToDelete);

            foreach (var node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
            OnValidate();
            Undo.DestroyObjectImmediate(nodeToDelete);
            
            EditorApplication.delayCall += CleanupOrphanNodes;
        }

        public void CleanupOrphanNodes()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);

            var referenced = new HashSet<DialogueNode>(nodes);

            foreach (var node in assets)
            {
                if (node is DialogueNode dialogueNode && !referenced.Contains(dialogueNode))
                {
                    DestroyImmediate(dialogueNode, allowDestroyingAssets: true);
                }
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            var newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            AssetDatabase.AddObjectToAsset(newNode, this);

            if (parentNode)
            {
                parentNode.AddChild(newNode.name);
                newNode.Position = parentNode.Position + new Vector2(250, 0);
            }

            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            
            EditorUtility.SetDirty(newNode);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void HandleUndoRedo()
        {
            onUndoRedoPerformed?.Invoke();
        }
#endif       

    }
}