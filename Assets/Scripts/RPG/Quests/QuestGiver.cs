using System;
using UnityEngine;
using RPG.Dialogues;
using RPG.Inventory;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private Quest quest;
        
        private QuestList questList;
        private QuestStatus questStatus;

        private void Awake()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onUpdated.AddListener(HandleQuestListUpdated);
        }

        private void OnDisable()
        {
            questList.onUpdated.RemoveListener(HandleQuestListUpdated);
        }

        public void GiveQuest()
        {
            questList.AddQuest(quest);
        }

        public void RewardQuest()
        {
            var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            foreach (var reward in quest.Rewards)
            {
                playerInventory.AddToFirstAvailableSlot(reward.item, reward.amount);
            }

            questStatus.SetProgress(QuestProgress.Rewarded);
        }
        
        private void HandleQuestListUpdated()
        {
            if (questList.HasQuest(quest))
            {
                questStatus = questList.GetStatus(quest);
            }
        }

    }
}
