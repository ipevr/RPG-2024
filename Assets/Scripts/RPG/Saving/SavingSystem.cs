using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            var state = LoadFile(saveFile);
            if (state.Count == 0) yield break;
            
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                var buildIndex = (int)state["lastSceneBuildIndex"];
                if (SceneManager.GetActiveScene().buildIndex != buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }

            RestoreState(state);
        }
        
        public void Save(string saveFile)
        {
            var state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            var state = LoadFile(saveFile);
            if (state.Count == 0) return;
            
            RestoreState(state);
        }

        private void SaveFile(string saveFile, object state)
        {
            var path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");
            using var stream = File.Open(path, FileMode.Create);
            
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            
            using var stream = File.Open(path, FileMode.Open);
            
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream) as Dictionary<string, object>;
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            var saveableEntities = FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None);
            foreach (var saveable in saveableEntities)
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                var uniqueIdentifier = saveable.GetUniqueIdentifier();
                if (!state.TryGetValue(uniqueIdentifier, out var value)) continue;
                
                saveable.RestoreState(value);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, $"{saveFile}.sav");
        }
    }
}