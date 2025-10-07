using System;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using RPG.UI.Inventory;

namespace RPG.UI
{
    public class UiController : MonoBehaviour
    {
        private List<PossessionUI> possessionUIs;
        private PlayerController playerController;

        private void Awake()
        {
            possessionUIs = new List<PossessionUI>();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            var myPossessionUIs = GetComponentsInChildren<PossessionUI>();
            foreach (var possessionUI in myPossessionUIs)
            {
                possessionUIs.Add(possessionUI);
            }
        }

        private void OnEnable()
        {
            foreach (var possessionUI in possessionUIs)
            {
                possessionUI.onDragging.AddListener(HandlePossessionUiDragging);
            }
        }

        private void OnDisable()
        {
            foreach (var possessionUI in possessionUIs)
            {
                possessionUI.onDragging.RemoveListener(HandlePossessionUiDragging);
            }
        }
        
        private void HandlePossessionUiDragging(bool isDragging)
        {
            playerController.AllowInteraction(!isDragging);
        }
        
    }
}