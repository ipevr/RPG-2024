using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class Trigger : MonoBehaviour
    {
        public UnityEvent onTriggerEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke();
        }
    }
}