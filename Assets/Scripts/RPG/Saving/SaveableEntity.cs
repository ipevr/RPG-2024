using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        private void Update()
        {
            if (Application.isPlaying || IsInPrefabMode()) return;

            SetUuidInScene();
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Debug.Log($"Capturing state for {GetUniqueIdentifier()}");
            return null;
        }

        public void RestoreState(object state)
        {
            Debug.Log($"Restoring state for {GetUniqueIdentifier()}");
        }

        private void SetUuidInScene()
        {
            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");
            if (!string.IsNullOrEmpty(property.stringValue)) return;
            
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        private bool IsInPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path);
        }
    }
}