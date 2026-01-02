using System;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] private Fighter[] fighters;
        [SerializeField] private bool activateOnStart = false;

        private void Start()
        {
            Activate(activateOnStart);
        }

        public void Activate(bool isActive)
        {
            foreach (var fighter in fighters)
            {
                var target = fighter.GetComponent<CombatTarget>();
                if (target)
                {
                    target.enabled = isActive;
                }
                fighter.enabled = isActive;
            }
        }
        
    }
}