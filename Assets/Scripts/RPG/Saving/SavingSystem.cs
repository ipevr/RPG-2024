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
            var path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");
            using var stream = File.Open(path, FileMode.Create);
            
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, CaptureState());
        }

        public void Load(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            using var stream = File.Open(path, FileMode.Open);
            
            var formatter = new BinaryFormatter();
            var state = formatter.Deserialize(stream);
            RestoreState(state);
        }

        private object CaptureState()
        {
            var state = new Dictionary<string, object>();
            var saveableEntities = FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None);
            foreach (var saveable in saveableEntities)
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            return state;
        }

        private void RestoreState(object state)
        {
            if (state is not Dictionary<string, object> stateDict) return;
            
            foreach (var saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                saveable.RestoreState(stateDict[saveable.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, $"{saveFile}.sav");
        }
    }
}