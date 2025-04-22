using System;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class AmountDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject amountObject;
        [SerializeField] private TextMeshProUGUI amountText;

        public void SetAmount(int amount)
        {
            if (amount > 1)
            {
                amountObject.SetActive(true);
                amountText.text = amount.ToString();
                return;
            }
            
            amountObject.SetActive(false);
        }
    }
}