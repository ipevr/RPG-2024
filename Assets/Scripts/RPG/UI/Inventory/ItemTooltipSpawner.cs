using UnityEngine;
using Utils.UI.Tooltips;

namespace RPG.UI.Inventory
{
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        protected override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>().GetItem();
            
            return item != null;
        }

        protected override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip) return;
            
            var item = GetComponent<IItemHolder>().GetItem();
            
            itemTooltip.Setup(item);
        }

    }
}