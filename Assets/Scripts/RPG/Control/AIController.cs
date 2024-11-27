using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            ProcessPlayerChaseBehaviour();
        }

        private void ProcessPlayerChaseBehaviour()
        {
            if (IsInChaseDistance(player))
            {
                Debug.Log($"{gameObject.name} is chasing.");
            }
        }

        private bool IsInChaseDistance(GameObject other)
        {
            return Vector3.Distance(other.transform.position, transform.position) < chaseDistance;
        }
    }
}