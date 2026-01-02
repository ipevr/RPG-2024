using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private InputActionReference toggleUI;
        [SerializeField] private InputActionReference hideUI;
        [SerializeField] private GameObject uiContainer;

        private void Start()
        {
            HideUI();
        }

        private void Update()
        {
            if (toggleUI && toggleUI.action.triggered)
            {
                Toggle();
            }
            
            if (hideUI && hideUI.action.triggered)
            {
                HideUI();
            }
        }

        public void HideUI()
        {
            uiContainer.SetActive(false);
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}