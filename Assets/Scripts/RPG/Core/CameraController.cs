using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private InputActionReference zoomAction;
        [SerializeField] private float zoomSensitivity = 0.01f;
        [SerializeField] private float startDistance = 10;
        [SerializeField] private float maxZoomIn = 2;
        [SerializeField] private float maxZoomOut = 20;

        private CinemachinePositionComposer composer;

        private void Awake()
        {
            composer = GetComponent<CinemachinePositionComposer>();
        }

        private void Start()
        {
            composer.CameraDistance = startDistance;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            if (zoomAction)
            {
                var scrollValue = zoomAction.action.ReadValue<float>();
                if (Mathf.Abs(scrollValue) > 0.1f)
                {
                    composer.CameraDistance -= scrollValue * zoomSensitivity;
                }
            }

            composer.CameraDistance = Mathf.Clamp(composer.CameraDistance, maxZoomIn, maxZoomOut);
        }

    }
}