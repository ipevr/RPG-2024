using System;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying || IsInPrefabMode()) return;

            SetUuidInScene();
        }
#endif

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            if (state is not SerializableVector3 position) return;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector3();
            GetComponent<NavMeshAgent>().enabled = true;
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