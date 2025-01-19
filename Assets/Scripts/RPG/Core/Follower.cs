using UnityEngine;

namespace RPG.Core
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform objectToFollow;
        [SerializeField] private bool followX;
        [SerializeField] private bool followY;
        [SerializeField] private bool followZ;

        private void Update()
        {
            transform.position = new Vector3(
                followX ? objectToFollow.position.x : transform.position.x,
                followY ? objectToFollow.position.y : transform.position.y,
                followZ ? objectToFollow.position.z : transform.position.z);
        }
    }
}