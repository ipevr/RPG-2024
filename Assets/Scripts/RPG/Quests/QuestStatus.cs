using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [System.Serializable]
        public struct QuestState
        {
            public string questId;
            public string[] completedObjectives;
            public string progress;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
            progress = QuestProgress.Started;
        }

        public QuestStatus(QuestState state)
        {
            quest = Quest.GetFromId(state.questId);
            completedObjectives = state.completedObjectives.ToList();
            progress = Enum.TryParse(state.progress, out QuestProgress result) ? result : QuestProgress.Started;
        }

        private Quest quest;
        private List<string> completedObjectives = new ();
        private QuestProgress progress;

        public event Action<QuestProgress> OnQuestProgressChanged;

        public Quest GetQuest()
        {
            return quest;
        }

        public List<string> GetCompletedObjectives()
        {
            return completedObjectives;
        }
        
        public QuestProgress GetProgress()
        {
            return progress;
        }
        
        public void SetProgress(QuestProgress newProgress)
        {
            progress = newProgress;
            OnQuestProgressChanged?.Invoke(progress);
        }
        
        public void CompleteObjective(string reference)
        {
            foreach (var questObjective in quest.Objectives)
            {
                if (reference == questObjective.reference)
                {
                    completedObjectives.Add(reference);
                }
            }
            
            if (IsCompleted())
            {
                SetProgress(QuestProgress.Completed);
            }
        }

        public int GetCompletedObjectivesCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveCompleted(string reference)
        {
            return completedObjectives.Contains(reference);
        }

        public bool IsCompleted()
        {
            return GetCompletedObjectivesCount() == quest.GetObjectiveCount();
        }

        public QuestState CaptureState()
        {
            return new QuestState
            {
                questId = quest.ID,
                completedObjectives = completedObjectives.ToArray(),
                progress = progress.ToString()
            };

        }
    }
}