using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Quests.Editor
{
    [CustomEditor(typeof(QuestObjectiveCompleter))]
    public class QuestObjectiveCompleterEditor : UnityEditor.Editor
    {
        private SerializedProperty objectiveReferenceProperty;
        private SerializedProperty questProperty;

        private void OnEnable()
        {
            objectiveReferenceProperty = serializedObject.FindProperty(QuestObjectiveCompleter.ObjectiveReferenceFieldName);
            questProperty = serializedObject.FindProperty(QuestObjectiveCompleter.QuestFieldName);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(questProperty);
            
            var quest = questProperty.objectReferenceValue as Quest;

            if (!quest)
            {
                EditorGUILayout.HelpBox("Please assign a Quest to see objectives.", MessageType.Info);
                return;
            }

            var objectiveReferences = new List<string>();
            
            foreach (var objective in quest.Objectives)
            {
                objectiveReferences.Add(objective.reference);
            }
            
            if (objectiveReferences.Count == 0)
            {
                EditorGUILayout.HelpBox("This quest has no objectives defined.", MessageType.Warning);
                return;
            }

            var questObjectiveCompleter = (QuestObjectiveCompleter)target;
            
            var currentIndex = objectiveReferences.IndexOf(questObjectiveCompleter.ObjectiveReference);
            
            if (currentIndex == -1) 
            {
                currentIndex = 0;
                objectiveReferenceProperty.stringValue = objectiveReferences[0];
            }

            var newIndex = EditorGUILayout.Popup("Objective", currentIndex, objectiveReferences.ToArray());

            if (newIndex != currentIndex)
            {
                Undo.RecordObject(questObjectiveCompleter, "Changed Quest Objective");
                objectiveReferenceProperty.stringValue = objectiveReferences[newIndex];
                EditorUtility.SetDirty(questObjectiveCompleter);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}