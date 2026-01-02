using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;

        private QuestStatus questStatus;
        
        public void Setup(QuestStatus status)
        {
            questStatus = status;
            title.text = status.Quest.Title;
            progress.text = $"{status.GetCompletedCount()}/{status.Quest.GetObjectiveCount()}";
        }
        
        public QuestStatus GetQuestStatus()
        {
            return questStatus;
        }
        
    }
}