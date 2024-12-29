using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string Extension = ".json";
        
        public IEnumerator LoadLastScene(string saveFile)
        {
            var state = LoadJsonFromFile(saveFile);
            IDictionary<string, JToken> stateDict = state;
            
            if (stateDict.ContainsKey("lastSceneBuildIndex"))
            {
                var buildIndex = (int)state["lastSceneBuildIndex"];
                if (SceneManager.GetActiveScene().buildIndex != buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }

            RestoreFromToken(state);
        }
        
        public void Save(string saveFile)
        {
            var state = LoadJsonFromFile(saveFile);
            CaptureAsToken(state);
            SaveFileAsJson(saveFile, state);
        }

        public void Load(string saveFile)
        {
            var state = LoadJsonFromFile(saveFile);
            
            RestoreFromToken(state);
        }

        private void SaveFileAsJson(string saveFile, JObject state)
        {
            var path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");
            using var textWriter = File.CreateText(path);
            using var writer = new JsonTextWriter(textWriter);
            writer.Formatting = Formatting.Indented;
            state.WriteTo(writer);
        }

        private JObject LoadJsonFromFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new JObject();
            }
            
            using var textReader = File.OpenText(path);
            using var reader = new JsonTextReader(textReader);
            reader.FloatParseHandling = FloatParseHandling.Double;
            return JObject.Load(reader);
        }

        private void CaptureAsToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (var saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJToken();
            }
            
            stateDict["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreFromToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (var saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                var uniqueIdentifier = saveable.GetUniqueIdentifier();
                if (!stateDict.TryGetValue(uniqueIdentifier, out var value)) continue;
                
                saveable.RestoreFromJToken(value);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, $"{saveFile}{Extension}");
        }
    }
}