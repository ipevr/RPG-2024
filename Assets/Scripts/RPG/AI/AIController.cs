using UnityEngine;
using UnityEngine.AI;
using Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 2f;
        [SerializeField] private float healthLosingDwellTime = 5f;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 2f;
        [SerializeField] private PatrolPath patrolPath;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = .2f;
        [SerializeField] private float maxNavPathLength = 10f; 
        
        private GameObject player;
        private Mover mover;
        private Fighter fighter;
        private Health health;

        private LazyValue<Vector3> guardPosition;
        private int currentWaypointIndex = 0;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private bool isBeingAttacked = false;
        private float currentHealthPoints;
        private float timeSinceLastBeingAttacked = Mathf.Infinity;

        #region Unity Event Functions

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            guardPosition = new LazyValue<Vector3>(InitializeGuardPosition);
        }

        private void Start()
        {
            guardPosition.ForceInit();
            currentHealthPoints = health.GetHealthPoints();
        }

        private void Update()
        {
            if (health.IsDead) return;

            CheckHealthLosing();
            
            ProcessPlayerChaseBehaviour();
            
            UpdateTimers();
        }

        #endregion
        
        #region Private Methods

        private void CheckHealthLosing()
        {
            if (health.GetHealthPoints() < currentHealthPoints)
            {
                timeSinceLastBeingAttacked = 0;
                currentHealthPoints = health.GetHealthPoints();
                isBeingAttacked = true;
            }
            else if (timeSinceLastBeingAttacked > healthLosingDwellTime)
            {
                isBeingAttacked = false;
            }
        }

        private Vector3 InitializeGuardPosition()
        {
            return transform.position;
        }
        
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastBeingAttacked += Time.deltaTime;
        }

        private void ProcessPlayerChaseBehaviour()
        {
            if ((IsInAttackRangeOfPlayer() && fighter.CanAttack(player)) || isBeingAttacked)
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            var nextPosition = guardPosition.value;
            
            if (patrolPath)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint(); 
            }

            if (timeSinceArrivedAtWaypoint >= waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private bool IsInAttackRangeOfPlayer()
        {
            var playerDistance = Vector3.Distance(player.transform.position, transform.position);
            if (playerDistance < chaseDistance && PathIsValid(player.transform.position))
            {
                isBeingAttacked = false;
                return true;
            }

            return false;
        }

        private bool PathIsValid(Vector3 target)
        {
            var path = new NavMeshPath();
            var pathExists = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!pathExists || path.status != NavMeshPathStatus.PathComplete) return false;

            return path.PathLength() <= maxNavPathLength;
        }
        
        #endregion

        #region Unity Callbacks

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        
        #endregion
    }
}