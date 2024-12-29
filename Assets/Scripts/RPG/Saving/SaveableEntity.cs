using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        
        private static readonly Dictionary<string, SaveableEntity> GlobalLookup = new();

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

        public JToken CaptureAsJToken()
        {
            var state = new JObject();
            IDictionary<string, JToken> stateDict = state;
            foreach (var saveable in GetComponents<ISaveable>())
            {
                var token = saveable.CaptureAsJToken();
                var component = saveable.GetType().ToString();
                stateDict[component] = token;
            }

            return state;
        }

        public void RestoreFromJToken(JToken token)
        {
            var state = token.ToObject<JObject>();
            IDictionary<string, JToken> stateDict = state;
            foreach (var saveable in GetComponents<ISaveable>())
            {
                var component = saveable.GetType().ToString();
                if (state != null && stateDict.TryGetValue(component, out var value))
                {
                    saveable.RestoreFromJToken(value);
                }
            }
        }

        private void SetUuidInScene()
        {
            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            
            GlobalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;
            
            if (GlobalLookup[candidate] == this) return true;

            if (GlobalLookup[candidate] == null)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            return false;
        }

        private bool IsInPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path);
        }
    }
}