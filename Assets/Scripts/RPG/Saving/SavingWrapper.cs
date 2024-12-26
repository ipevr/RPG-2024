using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private InputAction saveAction;
        [SerializeField] private InputAction loadAction;
        [SerializeField] private string saveFileName = "save";
        
        private SavingSystem savingSystem;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
        }

        private void OnEnable()
        {
            saveAction.Enable();
            loadAction.Enable();
        }

        private void OnDisable()
        {
            saveAction.Disable();
            loadAction.Disable();
        }

        private void Update()
        {
            HandleSaveAction();

            HandleLoadAction();
        }

        private void HandleLoadAction()
        {
            if (loadAction.triggered)
            {
                savingSystem.Load(saveFileName);
            }
        }

        private void HandleSaveAction()
        {
            if (saveAction.triggered)
            {
                savingSystem.Save(saveFileName);
            }
        }
    }
}