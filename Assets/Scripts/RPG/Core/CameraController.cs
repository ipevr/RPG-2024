using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private InputAction zoomInAction;
        [SerializeField] private InputAction zoomOutAction;
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

        private void OnEnable()
        {
            zoomInAction.Enable();
            zoomOutAction.Enable();
        }

        private void OnDisable()
        {
            zoomInAction.Disable();
            zoomOutAction.Disable();
        }

        private void Update()
        {
            if (zoomInAction.triggered)
            {
                composer.CameraDistance -= zoomInAction.ReadValue<float>();
            }
            else if (zoomOutAction.triggered)
            {
                composer.CameraDistance += zoomOutAction.ReadValue<float>();
            }
            composer.CameraDistance = Mathf.Clamp(composer.CameraDistance, maxZoomIn, maxZoomOut);
        }
    }
}