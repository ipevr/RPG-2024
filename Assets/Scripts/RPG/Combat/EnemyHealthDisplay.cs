using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthValue;
        
        private Fighter fighterComponent;
        
        private void Awake()
        {
            fighterComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            var enemyHealth = fighterComponent.GetTarget();
            healthValue.text = enemyHealth ? $"{enemyHealth.GetPercentage():0.0}%" : "N/A";
        }
    }
}