using UnityEngine;
using Utils.UI.Tooltips;

namespace RPG.UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        protected override bool CanCreateTooltip()
        {
            return true;
        }

        protected override void UpdateTooltip(GameObject tooltip)
        {
            var questStatus = GetComponent<QuestItemUI>().GetQuestStatus();

            if (questStatus != null)
            {
                tooltip.GetComponent<QuestTooltipUI>().Setup(questStatus);
            }
        }
    }
}