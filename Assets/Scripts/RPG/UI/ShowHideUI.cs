using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private InputActionReference toggleUI;
        [SerializeField] private GameObject uiContainer;

        private void Start()
        {
            uiContainer.SetActive(false);
        }

        private void Update()
        {
            if (toggleUI && toggleUI.action.triggered)
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}