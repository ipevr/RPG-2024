using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;
using Utils;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        
        [SerializeField] private LayerMask ignoredLayers;
        [SerializeField] private float normalSpeedFraction = 1f;
        [SerializeField] private float navMeshProjectionDistance = .1f;
        [FormerlySerializedAs("maxPathLength")] [SerializeField] private float maxNavPathLength = 10f;
        [SerializeField] private CursorMapping[] cursorMappings;
     
        private Health health;

        #region Unity Event Functions

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        #endregion
        
        #region Private Methods

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        private bool InteractWithComponent()
        {
            var hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                var raycastables = hit.transform.GetComponents<IRaycastable>();
                if (!PathIsValid(hit.transform.position))
                {
                    return false;
                }
                
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.RaycastAll(GetMouseRay(), Mathf.Infinity, ~ignoredLayers);
            return hits.OrderBy(hit => hit.distance).ToArray();
        }

        private bool InteractWithMovement()
        {
            // var hasHit = Physics.Raycast(GetMouseRay(), out var hit, Mathf.Infinity, ~ignoredLayers);
            var hasHit = RaycastNavMesh(out var target);
            if (!hasHit) return false;
            
            if (Input.GetMouseButton(0))
            {
                GetComponent<Mover>().StartMoveAction(target, normalSpeedFraction);
            }
            SetCursor(CursorType.Movement);
            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit, Mathf.Infinity, ~ignoredLayers);
            if (!hasHit) return false;
            
            var isOnNavMesh = NavMesh.SamplePosition(
                hit.point, out var navMeshHit, navMeshProjectionDistance, NavMesh.AllAreas);

            target = navMeshHit.position;

            return isOnNavMesh && PathIsValid(target);
        }

        private bool PathIsValid(Vector3 target)
        {
            var path = new NavMeshPath();
            var pathExists = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!pathExists || path.status != NavMeshPathStatus.PathComplete) return false;

            return path.PathLength() <= maxNavPathLength;
        }

        private void SetCursor(CursorType cursorType)
        {
            var mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == cursorType) return mapping;
            }

            return cursorMappings[0];
        }

        private Ray GetMouseRay()
        {
            var mainCamera = Camera.main;
            if (!mainCamera)
            {
                throw new MissingReferenceException("No main camera found");
            }
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        #endregion

    }
}
