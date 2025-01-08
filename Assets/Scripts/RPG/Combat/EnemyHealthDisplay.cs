using RPG.Attributes;
using RPG.Stats;
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

            if (enemyHealth)
            {
                healthValue.text = $"{enemyHealth.GetHealthPoints()} / {enemyHealth.GetMaxHealthPoints()}";
                return;
            }

            healthValue.text = "N/A";
        }
    }
}