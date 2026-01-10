using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        private readonly List<QuestStatus> statuses = new ();
        
        public IEnumerable<QuestStatus> Statuses => statuses;

        public UnityEvent onUpdated;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            
            var newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            newStatus.OnQuestProgressChanged += HandleQuestProgressChanged;
            
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

        public bool HasQuest(Quest quest)
        {
            return GetStatus(quest) != null;
        }
        
        public QuestStatus GetStatus(Quest quest)
        {
            foreach (var status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }

            return null;
        }
        
        private void HandleQuestProgressChanged(QuestProgress progress)
        {
            onUpdated?.Invoke();
        }

        #region Interface Implementations

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest":
                    return HasQuest(Quest.GetFromId(parameters[0]));
                case "HasNotQuest":
                    return !HasQuest(Quest.GetFromId(parameters[0]));
                case "HasQuestCompleted":
                {
                    var questStatus = GetStatus(Quest.GetFromId(parameters[0]));

                    if (questStatus != null)
                    {
                        return questStatus.GetProgress() == QuestProgress.Completed;
                    }

                    return false;
                }
                case "HasQuestRewarded":
                {
                    var questStatus = GetStatus(Quest.GetFromId(parameters[0]));

                    if (questStatus != null)
                    {
                        return questStatus.GetProgress() == QuestProgress.Rewarded;
                    }

                    return false;
                }
            }

            return null;
        }

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

                var status = new QuestStatus(questState);
                statuses.Add(status);
                status.OnQuestProgressChanged += HandleQuestProgressChanged;
            }
            
            onUpdated?.Invoke();
        }
        
        #endregion

    }
}