using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.UI
{
    public class HideAllUIs : MonoBehaviour
    {
        [SerializeField] private InputAction hideAllUIs;
        [SerializeField] private GameObject[] uiContainers;


        private void OnEnable()
        {
            hideAllUIs.Enable();
        }

        private void OnDisable()
        {
            hideAllUIs.Disable();
        }

        private void Update()
        {
            if (hideAllUIs.WasPerformedThisFrame())
            {
                HideAllUIContainers();
            }
        }

        private void HideAllUIContainers()
        {
            if (uiContainers != null)
            {
                foreach (var container in uiContainers)
                {
                    container.SetActive(false);
                }
            }
        }

    }
}