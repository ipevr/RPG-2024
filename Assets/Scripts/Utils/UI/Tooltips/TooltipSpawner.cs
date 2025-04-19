using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.UI.Tooltips
{
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject tooltipPrefab;

        private GameObject tooltip;

        private void OnDisable()
        {
            ClearTooltip();
        }

        private void OnDestroy()
        {
            ClearTooltip();
        }

        protected abstract void UpdateTooltip(GameObject tooltip);
        
        protected abstract bool CanCreateTooltip();
        
        private void ClearTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip.gameObject);
            }
        }

        private void PositionTooltip()
        {
            Canvas.ForceUpdateCanvases();
            
            var tooltipCorners = new Vector3[4];
            tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);
            
            var below = transform.position.y > Mathf.Abs(Screen.height / 2);
            var right = transform.position.x < Mathf.Abs(Screen.width / 2);

            var slotCorner = GetCornerIndex(below, right);
            var tooltipCorner = GetCornerIndex(!below, !right);
            
            tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            if (!below && !right) return 1;
            if (!below) return 2;
            return 3;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (tooltip && !CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!tooltip && CanCreateTooltip())
            {
                tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            if (tooltip)
            {
                UpdateTooltip(tooltip);
                PositionTooltip();
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }


    }
}
