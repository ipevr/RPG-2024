using System.Collections.Generic;
using System.Linq;

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
        }

        public QuestStatus(Quest quest)
        {
            Quest = quest;
        }

        public QuestStatus(QuestState state)
        {
            Quest = Quest.GetFromId(state.questId);
            CompletedObjectives = state.completedObjectives.ToList();
        }

        public Quest Quest { get; }
        public List<string> CompletedObjectives { get; } = new ();

        public void CompleteObjective(string reference)
        {
            foreach (var questObjective in Quest.Objectives)
            {
                if (reference == questObjective.reference)
                {
                    CompletedObjectives.Add(reference);
                }
            }
        }

        public int GetCompletedCount()
        {
            return CompletedObjectives.Count;
        }

        public bool IsObjectiveCompleted(string reference)
        {
            return CompletedObjectives.Contains(reference);
        }

        public QuestState CaptureState()
        {
            return new QuestState
            {
                questId = Quest.ID,
                completedObjectives = CompletedObjectives.ToArray()
            };

        }
    }
}