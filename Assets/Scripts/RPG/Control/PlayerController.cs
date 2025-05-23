﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
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
        [SerializeField] private float raycastRadius = 1f;
        [SerializeField] private CursorMapping[] cursorMappings;
     
        private Health health;
        private Mover mover;
        private bool interactionAllowed = true;

        #region Unity Event Functions

        private void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (!interactionAllowed) return;
            
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
        
        #region Public Methods

        public void AllowInteraction(bool status)
        {
            interactionAllowed = status;
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
            var hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius, Mathf.Infinity, ~ignoredLayers);
            return hits.OrderBy(hit => hit.distance).ToArray();
        }

        private bool InteractWithMovement()
        {
            var hasHit = RaycastNavMesh(out var target);
            if (!hasHit || !mover.CanMoveTo(target)) return false;

            if (Input.GetMouseButton(0))
            {
                mover.StartMoveAction(target, normalSpeedFraction);
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

            return isOnNavMesh;
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
