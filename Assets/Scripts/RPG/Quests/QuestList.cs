using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
    {
        private readonly List<QuestStatus> statuses = new ();
        
        public IEnumerable<QuestStatus> Statuses => statuses;

        public UnityEvent onUpdated;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            
            var newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            
            onUpdated?.Invoke();
        }

        public void CompleteObjective(Quest quest, string reference)
        {
            if (!HasQuest(quest)) return;
            
            var status = GetStatus(quest);

            if (!quest.HasObjective(reference)) return;
            
            status.CompleteObjective(reference);
                    
            onUpdated?.Invoke();
        }

        private QuestStatus GetStatus(Quest quest)
        {
            foreach (var status in statuses)
            {
                if (status.Quest == quest)
                {
                    return status;
                }
            }

            return null;
        }

        private bool HasQuest(Quest quest)
        {
            return GetStatus(quest) != null;
        }
        
        #region Interface Implementations


        public JToken CaptureAsJToken()
        {
            var state = new JArray();
            
            foreach (var status in statuses)
            {
                var questToSave = status.CaptureState();
                state.Add(JToken.FromObject(questToSave));
            }

            return state;
        }

        public void RestoreFromJToken(JToken state)
        {
            if (state is not JArray jArray) return;
            
            statuses.Clear();
            
            foreach (var token in jArray)
            {
                var questState = token.ToObject<QuestStatus.QuestState>();

                statuses.Add(new QuestStatus(questState));
            }
            
            onUpdated?.Invoke();
        }
        
        #endregion
    }
}