using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private Button detailsButton;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;
        [SerializeField] private QuestDetailsUI questDetailsPrefab;
        
        private QuestStatus questStatus;
        
        public void Setup(QuestStatus status)
        {
            questStatus = status;
            title.text = status.Quest.Title;
            progress.text = $"{status.GetCompletedCount()}/{status.Quest.GetObjectiveCount()}";
            detailsButton.onClick.AddListener(HandleDetailsButtonClicked);
        }

        private void HandleDetailsButtonClicked()
        {
            var parentCanvas = GetComponentInParent<Canvas>();
            DestroyQuestDetails(parentCanvas);
            var questDetailsUi = Instantiate(questDetailsPrefab, parentCanvas.transform);
            questDetailsUi.Setup(questStatus);
        }

        private void DestroyQuestDetails(Canvas canvas)
        {
            foreach (var details in canvas.GetComponentsInChildren<QuestDetailsUI>())
            {
                Destroy(details.gameObject);
            }
        }
    }
}