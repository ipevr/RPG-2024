using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogues
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool isPlayerSpeaking;
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new();
        [SerializeField] private Rect rect = new (10, 10, 200, 150);
        [SerializeField] private DialogueAction onEnterAction = DialogueAction.None;
        [SerializeField] private DialogueAction onExitAction = DialogueAction.None;

        public Rect Rect => rect;
        public IEnumerable<string> Children => children;
        public string Text => text;
        public bool IsPlayerSpeaking => isPlayerSpeaking;
        public DialogueAction OnEnterAction => onEnterAction;
        public DialogueAction OnExitAction => onExitAction;
        
#if UNITY_EDITOR        
        public UnityEvent onUndoRedoPerformed = new ();
        
        private void OnEnable()
        {
            Undo.undoRedoPerformed += HandleUndoRedo;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= HandleUndoRedo;
        }

        public void SetPlayerSpeaking(bool status)
        {
            Undo.RecordObject(this, "Changed Dialogue Speaker");
            isPlayerSpeaking = status;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Changed Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public Vector2 Position
        {
            get => rect.position;
            set
            {
                Undo.RegisterCompleteObjectUndo(this, "Changed Dialogue Position");
                rect.position = value;
                EditorUtility.SetDirty(this);
            }
        }
        
        public void AddChild(string childId)
        {
            Undo.RecordObject(this, "Linked Dialogue Node");
            children.Add(childId);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childId)
        {
            Undo.RecordObject(this, "Unlinked Dialogue Node");
            children.Remove(childId);
            EditorUtility.SetDirty(this);
        }

        private void HandleUndoRedo()
        {
            onUndoRedoPerformed?.Invoke();
        }
#endif
        
    }
} 