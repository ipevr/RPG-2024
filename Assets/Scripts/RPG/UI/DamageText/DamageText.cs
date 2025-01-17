using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageDisplay;

        public void ShowDamage(float damage)
        {
            damageDisplay.text = $"{damage:0}";
        }

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}