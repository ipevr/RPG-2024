using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent onMakingDamage;
        
        public void HandleMakingDamage()
        {
            onMakingDamage.Invoke();
        }

    }
    
}