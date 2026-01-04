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
        
        private QuestList questList;
        private QuestStatus questStatus;
        
        private void Awake()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        }

        private void OnEnable()
        {
            closeButton.onClick.AddListener(HandleCloseButtonClicked);
            questList.onUpdated.AddListener(Redraw);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(HandleCloseButtonClicked);
            questList.onUpdated.RemoveListener(Redraw);
        }

        public void Setup(QuestStatus status)
        {
            questStatus = status;
        }

        public void Redraw()
        {
            DestroyInstantiations();
            
            title.text = questStatus.GetQuest().Title;

            foreach (var objective in questStatus.GetQuest().Objectives)
            {
                var isCompleted = questStatus.IsObjectiveCompleted(objective.reference);
                var objectivePrefab = isCompleted ? objectiveCompletedPrefab : objectiveOpenPrefab;
                var objectiveItem = Instantiate(objectivePrefab, objectiveList);
                
                objectiveItem.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }
            
            foreach (var reward in questStatus.GetQuest().Rewards)
            {
                var rewardUi = Instantiate(rewardPrefab, rewardsList);
                rewardUi.Setup(reward.item);
            }
        }

        private void DestroyInstantiations()
        {
            foreach (Transform child in objectiveList)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in rewardsList)
            {
                Destroy(child.gameObject);
            }
        }

        private void HandleCloseButtonClicked()
        {
            Destroy(gameObject);
        }

    }
}