using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using RPG.Core;
using Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 2f;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 2f;
        [SerializeField] private PatrolPath patrolPath;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = .2f;
        
        private GameObject player;
        private Mover mover;
        private Fighter fighter;
        private Health health;

        private LazyValue<Vector3> guardPosition;
        private int currentWaypointIndex = 0;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;

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
        }

        private void Update()
        {
            if (health.IsDead) return;
            
            ProcessPlayerChaseBehaviour();
            
            UpdateTimers();
        }

        #endregion
        
        #region Private Methods

        private Vector3 InitializeGuardPosition()
        {
            return transform.position;
        }
        
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void ProcessPlayerChaseBehaviour()
        {
            if (IsInAttackRangeOfPlayer() && fighter.CanAttack(player))
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
            return playerDistance < chaseDistance;
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