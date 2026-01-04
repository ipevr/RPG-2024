using UnityEngine;

namespace RPG.Dialogues
{
    public interface IConversant
    {
        Dialogue GetDialogue();
        string GetName();
        GameObject GetGameObject();
        void StartDialogue();
    }
}