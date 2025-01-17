using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthValue;
        
        private Health healthComponent;
        
        private void Awake()
        {
            healthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            // healthValue.text = $"{healthComponent.GetPercentage():0.0}%";
            healthValue.text = $"{healthComponent.GetHealthPoints()} / {healthComponent.GetMaxHealthPoints()}";
        }
    }
}