using UnityEngine;
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
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 2f;
        [SerializeField] private float aggroCooldownTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float mobRadius = 5f;
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
        private float timeSinceLastAggravation = Mathf.Infinity;
        private bool isDirectlyAggravated;

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
            
            if (IsAggravated() && fighter.CanAttack(player))
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

            UpdateTimers();
        }

        #endregion

        #region Public Methods

        public void Aggravate(bool directly = true)
        {
            timeSinceLastAggravation = 0;
            isDirectlyAggravated = directly;
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
            timeSinceLastAggravation += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            if (isDirectlyAggravated)
            {
                ActivateMobs();
            }
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

        private bool IsAggravated()
        {
            var playerDistance = Vector3.Distance(player.transform.position, transform.position);

            if (playerDistance < chaseDistance || timeSinceLastAggravation < aggroCooldownTime)
            {
                return true;
            }

            isDirectlyAggravated = false;
            
            return false;
        }

        private void ActivateMobs()
        {
            var hits = Physics.SphereCastAll(transform.position, mobRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                var mobMember = hit.collider.GetComponent<AIController>();
                if (mobMember && !mobMember.IsAggravated())
                {
                    Debug.Log($"Aggravating {mobMember.name}");
                    mobMember.Aggravate(false);
                }
            }
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