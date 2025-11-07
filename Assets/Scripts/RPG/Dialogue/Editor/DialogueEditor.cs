using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue currentDialogue;
        private GUIStyle nodeStyle;
        private DialogueNode draggingNode;
        private Vector2 draggingOffset;
        private DialogueNode linkingNode;

        [MenuItem("RPG/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line, int column)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is Dialogue)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        private void Awake()
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, new Color(0.18f, 0.18f, 0.18f, 1));
            tex.Apply();

            nodeStyle = new GUIStyle
            {
                normal = {background = EditorGUIUtility.Load("node0") as Texture2D},
                padding = new RectOffset(12, 12, 12, 12),
                border = new RectOffset(12, 12, 12, 12)
            };

            OnSelectionChange();
            
            Undo.undoRedoPerformed += Repaint;
        }

        private void OnGUI()
        {
            if (!currentDialogue)
            {
                EditorGUILayout.HelpBox("No dialogue selected. Open a dialogue asset to edit.", MessageType.Info);
                return;
            }
            
            foreach (var node in currentDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }
            foreach (var node in currentDialogue.GetAllNodes())
            {
                DrawNode(node);
            }

            ProcessEvents();

        }

        private void DrawConnections(DialogueNode node)
        {
            foreach (var childNode in currentDialogue.GetAllChildren(node))
            {
                DrawConnection(node, childNode);
            }
            
        }
        
        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.Rect, GUIContent.none, nodeStyle);
            EditorGUI.BeginChangeCheck();

            var newText = EditorGUILayout.TextField(node.Text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(currentDialogue, "Changed Dialogue Text");
                node.Text = newText;
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                EditorApplication.delayCall += () => CreateNewNode(node);
            }

            DrawLinkButton(node);

            if (GUILayout.Button("-"))
            {
                EditorApplication.delayCall += () => DeleteNode(node);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void ProcessEvents()
        {
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(currentEvent.mousePosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.Rect.position - currentEvent.mousePosition;
                }
            }
            else if (currentEvent.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RegisterCompleteObjectUndo(currentDialogue, "Changed Dialogue Position");
                draggingNode.SetPosition(currentEvent.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (currentEvent.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
        }

        private void DrawConnection(DialogueNode fromNode, DialogueNode toNode)
        {
            const int width = 2;
            const float tangentFraction = .8f;
            var startPoint = new Vector2(fromNode.Rect.xMax, fromNode.Rect.center.y);
            var endPoint = new Vector2(toNode.Rect.xMin, toNode.Rect.center.y);
            var tanLength = Mathf.Abs(startPoint.x - endPoint.x) * tangentFraction;
            var startTangent = startPoint + Vector2.right * tanLength;
            var endTangent = endPoint + Vector2.left * tanLength;
            
            Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.white, null, width);
        }

        private void CreateNewNode(DialogueNode parentNode)
        {
            Undo.RecordObject(currentDialogue, "Added Dialogue Node");
            currentDialogue.CreateNode(parentNode);
            Repaint();
        }

        private void DrawLinkButton(DialogueNode node)
        {
            if (linkingNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        linkingNode = node;
                        Repaint();
                    };
                }
            }
            else if (linkingNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        linkingNode = null;
                        Repaint();
                    };
                }
            }
            else if (linkingNode.Children.Contains(node.UniqueId))
            {
                if (GUILayout.Button("unlink"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        linkingNode.RemoveChild(node.UniqueId);
                        linkingNode = null;
                        Repaint();
                    };
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        linkingNode.AddChild(node.UniqueId);
                        linkingNode = null;
                        Repaint();
                    };
                }
            }
            
        }

        private void DeleteNode(DialogueNode node)
        {
            Undo.RecordObject(currentDialogue, "Deleted Dialogue Node");
            currentDialogue.DeleteNode(node);
            Repaint();
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (var node in currentDialogue.GetAllNodes())
            {
                if (node.Rect.Contains(point))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is Dialogue dialogue)
            {
                currentDialogue = dialogue;
                Repaint();
            }
        }

    }
}