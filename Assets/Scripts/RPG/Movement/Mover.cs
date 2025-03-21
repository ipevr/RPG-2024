using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;
using Utils;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private static readonly int ForwardSpeedId = Animator.StringToHash("forward speed");

        [SerializeField] private float maxSpeed = 4f;
        [SerializeField] private float maxNavPathLength = 10f;

        private NavMeshAgent navMeshAgent;

        #region Unity Event Functions

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            ProcessAnimation();
        }

        #endregion

        #region Public Methods

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

        public bool CanMoveTo(Vector3 destination)
        {
            var path = new NavMeshPath();
            var pathExists = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!pathExists || path.status != NavMeshPathStatus.PathComplete) return false;

            return path.PathLength() <= maxNavPathLength;
        }

        #endregion
        
        #region Private Methods
        
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
        
        #endregion

        #region Animation Events

        // Animation Event
        private void FootL()
        {
        }

        // Animation Event
        private void FootR()
        {
        }

        #endregion

        #region Interface implementations

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            navMeshAgent.enabled = false;
            transform.position = state.ToVector3();
            navMeshAgent.enabled = true;
        }

        #endregion
    }
}