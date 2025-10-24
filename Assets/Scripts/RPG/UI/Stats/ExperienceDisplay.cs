using TMPro;
using UnityEngine;
using RPG.Stats;

namespace RPG.UI.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI experienceValue;
        
        private Experience experienceComponent;
        
        private void Awake()
        {
            experienceComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            experienceValue.text = $"{experienceComponent.ExperiencePoints}";
        }
    }
}