using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private InputAction saveAction;
        [SerializeField] private InputAction loadAction;
        [SerializeField] private string saveFileName = "save";
        [SerializeField] private float fadeTime = 1f;
        
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

        private IEnumerator Start()
        {
            var fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();
            
            yield return savingSystem.LoadLastScene(saveFileName);

            yield return fader.FadeIn(fadeTime);
        }

        private void Update()
        {
            HandleSaveAction();

            HandleLoadAction();
        }

        public void Load()
        {
            savingSystem.Load(saveFileName);
        }

        public void Save()
        {
            savingSystem.Save(saveFileName);
        }

        private void HandleLoadAction()
        {
            if (loadAction.triggered)
            {
                Load();
            }
        }

        private void HandleSaveAction()
        {
            if (saveAction.triggered)
            {
                Save();
            }
        }

    }
}