using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Coroutine currentActiveFade;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1f;
        }

        public Coroutine FadeIn(float fadingTime)
        {
            return Fade(0f, fadingTime);
        }

        public Coroutine FadeOut(float fadingTime)
        {
            return Fade(1f, fadingTime);
        }

        private Coroutine Fade(float alphaTarget, float fadingTime)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(alphaTarget, fadingTime));
            return currentActiveFade;
        }
        
        private IEnumerator FadeRoutine(float alphaTarget, float fadingTime)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, alphaTarget))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / fadingTime);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}