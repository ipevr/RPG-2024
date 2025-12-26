using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.UI
{
    public class HideAllUIs : MonoBehaviour
    {
        [SerializeField] private InputActionReference hideAllUIs;
        [SerializeField] private GameObject[] uiContainers;

        private void Update()
        {
            if (hideAllUIs && hideAllUIs.action.triggered)
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