﻿using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab;

        public void Spawn(float damage)
        {
            var damageText = Instantiate(damageTextPrefab, transform);
            damageText.ShowDamage(damage);
        }
    }
}