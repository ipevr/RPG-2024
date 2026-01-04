using System.Collections.Generic;
using RPG.Dialogues;
using UnityEngine;
using RPG.Inventory;

namespace RPG.Quests
{
    [CreateAssetMenu(menuName = "RPG/Quests/Quest", order = 0)]
    public class Quest : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID for saving / loading. Clear this field if you want to generate a new one.")]
        [SerializeField] private string id;
        [Tooltip("The objectives to be completed for completing this quest.")]
        [SerializeField] private List<Objective> objectives = new();
        [SerializeField] private List<Reward> rewards = new();
        [Tooltip("The dialogue to be played to start the quest.")]
        [SerializeField] private Dialogue questStartDialogue;
        [Tooltip("The dialogue to be played when the quest is not completed yet.")]
        [SerializeField] private Dialogue questStartedDialogue;
        [Tooltip("The dialogue to be played when the player delivers the completed quest to the quest giver.")]
        [SerializeField] private Dialogue questCompletedDialogue;
        [Tooltip("The dialogue to be played when the player has finished the quest and already got the quest rewards.")]
        [SerializeField] private Dialogue questRewardedDialogue;

        [System.Serializable]
        public class Reward
        {
            public int amount;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }
        
        public string ID => id;
        public string Title => name;
        public IEnumerable<Objective> Objectives => objectives;
        public List<Reward> Rewards => rewards;
        public Dialogue QuestStartDialogue => questStartDialogue;
        public Dialogue QuestStartedDialogue => questStartedDialogue;
        public Dialogue QuestCompletedDialogue => questCompletedDialogue;
        public Dialogue QuestRewardedDialogue => questRewardedDialogue;

        public static Quest GetFromId(string questId)
        {
            foreach (var quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.id == questId) return quest;
            }
            
            Debug.LogError($"Quest with id {questId} not found!");

            return null;
        }
        
        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public Objective GetObjective(string reference)
        {
            foreach (var objective in objectives)
            {
                if (objective.reference == reference) return objective;
            }

            return null;
        }
        
        public bool HasObjective(string reference)
        {
            foreach (var questObjective in Objectives)
            {
                if (reference == questObjective.reference) return true;
            }

            return false;
        }
        
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}