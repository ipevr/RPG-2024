using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        [SerializeField] private string uniqueId = Guid.NewGuid().ToString();
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new();
        [SerializeField] private Rect rect = new (10, 10, 200, 150);

        public string UniqueId => uniqueId;

        public Rect Rect
        {
            get => rect;
            private set => rect = value;
        }

        public string Text
        {
            get => text; 
            set => text = value;
        }

        public IEnumerable<string> Children => children;

        public void AddChild(string childId)
        {
            children.Add(childId);
        }

        public void RemoveChild(string childId)
        {
            children.Remove(childId);
        }

        public void SetPosition(Vector2 position)
        {
            Rect = new Rect(position, rect.size);
        }
    }
}