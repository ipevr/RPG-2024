using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private InputAction toggleUI;
        [SerializeField] private GameObject uiContainer;

        private void Start()
        {
            uiContainer.SetActive(false);
        }

        private void OnEnable()
        {
            toggleUI.Enable();
        }

        private void OnDisable()
        {
            toggleUI.Disable();
        }

        private void Update()
        {
            if (toggleUI.WasPerformedThisFrame())
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}