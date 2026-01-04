using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Quests;
using RPG.UI.Inventory;

namespace RPG.UI.Quests
{
    public class QuestDetailsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private GameObject objectiveOpenPrefab;
        [SerializeField] private GameObject objectiveCompletedPrefab;
        [SerializeField] private RewardUi rewardPrefab;
        [SerializeField] private Transform objectiveList;
        [SerializeField] private Transform rewardsList;
        [SerializeField] private Button closeButton;
        
        private void OnEnable()
        {
            closeButton.onClick.AddListener(HandleCloseButtonClicked);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(HandleCloseButtonClicked);
        }

        public void Setup(QuestStatus status)
        {
            title.text = status.GetQuest().Title;

            foreach (var objective in status.GetQuest().Objectives)
            {
                var isCompleted = status.IsObjectiveCompleted(objective.reference);
                var objectivePrefab = isCompleted ? objectiveCompletedPrefab : objectiveOpenPrefab;
                var objectiveItem = Instantiate(objectivePrefab, objectiveList);
                
                objectiveItem.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }
            
            foreach (var reward in status.GetQuest().Rewards)
            {
                var rewardUi = Instantiate(rewardPrefab, rewardsList);
                rewardUi.Setup(reward.item);
            }
        }

        private void HandleCloseButtonClicked()
        {
            Destroy(gameObject);
        }

    }
}