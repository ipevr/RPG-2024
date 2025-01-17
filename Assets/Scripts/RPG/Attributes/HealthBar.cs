using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health healthComponent;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private GameObject canvas;

        private void Start()
        {
            canvas.SetActive(false);
        }

        public void HandleDamage(float damage)
        {
            canvas.SetActive(true);
            foreground.localScale = new Vector3(
                healthComponent.GetFraction(),
                foreground.localScale.y,
                foreground.localScale.z);
        }

        public void HandleDeath()
        {
            canvas.SetActive(false);
        }
    }
}