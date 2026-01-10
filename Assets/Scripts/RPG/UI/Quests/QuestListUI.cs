using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private QuestItemUI questItemPrefab;

        private QuestList questList;

        private void Awake()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        }

        private void OnEnable()
        {
            questList.onUpdated.AddListener(Redraw);
            Redraw();
        }

        private void OnDisable()
        {
            questList.onUpdated.RemoveListener(Redraw);
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            transform.DetachChildren();
            
            foreach (var questStatus in questList.Statuses)
            {
                if (questStatus.GetProgress() == QuestProgress.Rewarded) continue;
                
                var questItem = Instantiate(questItemPrefab, transform);
                questItem.Setup(questStatus);
            }
        }

    }
}