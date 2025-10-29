using UnityEngine;
using UnityEngine.AI;
using RPG.Stats;

namespace RPG.Pickups
{
    public class RandomItemDropper : ItemDropper
    {
        [Tooltip( "This can be left empty for the player" )]
        [SerializeField] private DropTable dropTable;
        [Tooltip( "The distance from the dropper to the dropped item" )]
        [SerializeField] private float dropDistance = 1f;
        [SerializeField] private float maxNavMeshProjectionDistance = .1f;

        private const int Attempts = 30;
        
        public void RandomDrop()
        {
            if (!dropTable) return;
            
            var currentLevel = GetComponent<BaseStats>().GetLevel();

            foreach (var (item, amount) in dropTable.RollDrops(currentLevel))
            {
                Drop(item, amount);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            for (var i = 0; i < Attempts; i++)
            {
                var randomDirection = Random.insideUnitCircle.normalized;
                var offset = new Vector3(randomDirection.x, 0f, randomDirection.y) * dropDistance;
                var candidate = transform.position + offset;
                
                if (NavMesh.SamplePosition(candidate, out var hit, maxNavMeshProjectionDistance, NavMesh.AllAreas))
                {
                    Debug.Log($"Distance from dropper: {Vector3.Distance(transform.position, hit.position)}");
                    return hit.position;
                }
            }
            Debug.LogWarning("Could not find a valid drop location");
            return transform.position;
        }


    }
}