﻿using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelValue;
        
        private BaseStats baseStatsComponent;
        
        private void Awake()
        {
            baseStatsComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            levelValue.text = $"{baseStatsComponent.GetLevel()}";
        }

    }
}