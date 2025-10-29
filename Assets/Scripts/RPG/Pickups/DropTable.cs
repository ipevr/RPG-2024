using System.Collections.Generic;
using UnityEngine;
using RPG.Inventory;

namespace RPG.Pickups
{
    [CreateAssetMenu(menuName = "RPG/Loot/Drop Table", order = 0)]
    public class DropTable : ScriptableObject
    {
        [System.Serializable]
        public class Drop
        {
            public InventoryItem item;
            public int minAmount = 1;
            public int maxAmount = 1;
            public float dropChance = 1f;
            public int GetRandomAmount()
            {
                return item.IsStackable ? Random.Range(minAmount, maxAmount + 1) : 1;
            }
        }

        [System.Serializable]
        public class LevelDrops
        {
            [Min(1)]
            public int level = 1;
            public Drop[] drops;
        }

        [SerializeField] private List<LevelDrops> levelBuckets = new();
        [SerializeField] private int minDrops;
        [SerializeField] private int maxDrops = 2;

        public IEnumerable<(InventoryItem item, int amount)> RollDrops(int level)
        {
            var bucket = GetBucketForLevel(level);
            if (bucket?.drops == null || bucket.drops.Length == 0)
                yield break;
            
            var dropsToRoll = Random.Range(minDrops, maxDrops + 1);
            for (var i = 0; i < dropsToRoll; i++)
            {
                var chosenDrop = RollDrop(bucket.drops);
                if (chosenDrop == null || chosenDrop.item == null) continue;

                yield return (chosenDrop.item, chosenDrop.GetRandomAmount());
            }
        }

        private LevelDrops GetBucketForLevel(int level)
        {
            if (levelBuckets == null || levelBuckets.Count == 0) return null;
            
            // Pick the exact match, otherwise the highest bucket with level <= requested level.
            LevelDrops bestBucket = null;
            foreach (var bucket in levelBuckets)
            {
                if (bucket == null) continue;
                if (bucket.level == level) return bucket;
                if (bucket.level <= level && (bestBucket == null || bucket.level > bestBucket.level))
                {
                    bestBucket = bucket;
                }
            }

            return bestBucket ?? levelBuckets[0];
        }

        private Drop RollDrop(Drop[] drops)
        {
            var totalDropChance = GetTotalDropChance(drops);
            if (totalDropChance <= 0f)
            {
                return null;
            }

            var dropChance = Random.Range(0f, totalDropChance);
            var cumulativeDropChance = 0f;
            foreach (var drop in drops)
            {
                var clampedDropChance = Mathf.Max(0f, drop.dropChance);
                cumulativeDropChance += clampedDropChance;
                if (dropChance < cumulativeDropChance)
                {
                    return drop;
                }
            }
            
            return drops.Length > 0 ? drops[^1] : null;
        }

        private float GetTotalDropChance(Drop[] drops)
        {
            var chance = 0f;
            foreach (var drop in drops)
            {
                chance += Mathf.Max(0f, drop.dropChance);
            }
            
            return chance;
        }
    }
}