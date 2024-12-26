using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            SaveFile(saveFile, CaptureState());
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
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
            using var stream = File.Open(path, FileMode.Open);
            
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream) as Dictionary<string, object>;
        }

        private Dictionary<string, object> CaptureState()
        {
            var state = new Dictionary<string, object>();
            var saveableEntities = FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None);
            foreach (var saveable in saveableEntities)
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            return state;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                saveable.RestoreState(state[saveable.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, $"{saveFile}.sav");
        }
    }
}