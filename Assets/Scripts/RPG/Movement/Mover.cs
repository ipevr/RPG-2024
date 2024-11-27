using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private static readonly int ForwardSpeedId = Animator.StringToHash("forward speed");
    
        private void Update()
        {
            ProcessAnimation();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination, float stoppingDistance = 0)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
            GetComponent<NavMeshAgent>().destination = destination;
        }

        public void Cancel()
        {
            GetComponent<NavMeshAgent>().isStopped = true;
        }

        private void ProcessAnimation()
        {
            var speed = CalculateSpeed();
            GetComponent<Animator>().SetFloat(ForwardSpeedId, speed);
        }

        private float CalculateSpeed()
        {
            var velocity = GetComponent<NavMeshAgent>().velocity;
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