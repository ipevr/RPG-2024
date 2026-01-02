using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private GameObject objectiveOpenPrefab;
        [SerializeField] private GameObject objectiveCompletedPrefab;
        [SerializeField] private Transform objectiveList;

        public void Setup(QuestStatus status)
        {
            title.text = status.Quest.Title;

            foreach (var objective in status.Quest.Objectives)
            {
                var isCompleted = status.IsObjectiveCompleted(objective.reference);
                var objectivePrefab = isCompleted ? objectiveCompletedPrefab : objectiveOpenPrefab;
                var objectiveItem = Instantiate(objectivePrefab, objectiveList);
                
                objectiveItem.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }
        }

    }
}