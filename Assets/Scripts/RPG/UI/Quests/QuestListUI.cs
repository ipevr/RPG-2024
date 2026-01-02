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
                var questItem = Instantiate(questItemPrefab, transform);
                questItem.Setup(questStatus);
            }
        }

    }
}