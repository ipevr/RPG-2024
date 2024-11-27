using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;
        
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                Debug.Log($"Stopping action: {currentAction}");
                currentAction.Cancel();
            }

            currentAction = action;
        }

    }
}