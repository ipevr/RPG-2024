using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private static readonly int ForwardSpeedId = Animator.StringToHash("forward speed");

        [SerializeField] private float maxSpeed = 4f;
        
        private NavMeshAgent navMeshAgent;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            ProcessAnimation();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void ProcessAnimation()
        {
            var speed = CalculateSpeed();
            GetComponent<Animator>().SetFloat(ForwardSpeedId, speed);
        }

        private float CalculateSpeed()
        {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            return speed;
        }

        // Animation Event
        private void FootL()
        {
        }

        // Animation Event
        private void FootR()
        {
        }

    }
}