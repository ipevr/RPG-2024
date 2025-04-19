using RPG.Inventory;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void Setup(InventoryItem item)
        {
            titleText.text = item.DisplayName;
            descriptionText.text = item.Description;
        }
    }
}