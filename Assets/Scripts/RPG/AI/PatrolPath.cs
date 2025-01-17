using UnityEngine;

namespace RPG.AI
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float waypointSphereRadius = 0.2f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            for (var i = 0; i < transform.childCount; i++)
            {
                var j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointSphereRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(j));
            }
        }

        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if (i < transform.childCount - 1)
            {
                return i + 1;
            }
            return 0;
        }
    }
}