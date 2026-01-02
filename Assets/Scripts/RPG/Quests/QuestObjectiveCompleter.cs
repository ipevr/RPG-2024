using UnityEngine;

namespace RPG.Quests
{
    public class QuestObjectiveCompleter : MonoBehaviour
    {
        [SerializeField] private Quest quest;
        [SerializeField] private string objectiveReference;

        public string ObjectiveReference => objectiveReference;

        public static string QuestFieldName => nameof(quest);
        public static string ObjectiveReferenceFieldName => nameof(objectiveReference);
        
        public void Complete()
        {
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            
            questList.CompleteObjective(quest, objectiveReference);
        }
    }
}