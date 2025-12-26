using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private InputActionReference saveAction;
        [SerializeField] private InputActionReference loadAction;
        [SerializeField] private InputActionReference deleteAction;
        [SerializeField] private string saveFileName = "save";
        [SerializeField] private float fadeTime = 1f;
        
        private SavingSystem savingSystem;

        #region Unity Event Functions

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private void Update()
        {
            HandleSaveAction();
            HandleLoadAction();
            HandleDeleteAction();
        }

        #endregion

        #region Public Methods

        public void Load()
        {
            savingSystem.Load(saveFileName);
        }

        public void Save()
        {
            savingSystem.Save(saveFileName);
        }

        public void Delete()
        {
            savingSystem.Delete(saveFileName);
        }

        #endregion
        
        #region Private Methods

        private IEnumerator LoadLastScene()
        {
            yield return savingSystem.LoadLastScene(saveFileName);
 
            var fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();

            yield return fader.FadeIn(fadeTime);
        }

        private void HandleLoadAction()
        {
            if (loadAction && loadAction.action.triggered)
            {
                Load();
            }
        }

        private void HandleSaveAction()
        {
            if (saveAction && saveAction.action.triggered)
            {
                Save();
            }
        }

        private void HandleDeleteAction()
        {
            if (deleteAction && deleteAction.action.triggered)
            {
                Delete();
            }
        }

        #endregion
    }
}