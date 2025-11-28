using System;
using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI aiText;
        [SerializeField] private Button nextButton;
        
        private PlayerConversant playerConversant;

        private void Awake()
        {
            playerConversant = PlayerConversant.GetPlayerConversant();
        }

        private void OnEnable()
        {
            nextButton.onClick.AddListener(HandleNextButtonClicked);
        }

        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(HandleNextButtonClicked);
        }

        private void Start()
        {
            ShowCurrentText();
        }

        private void ShowCurrentText()
        {
            aiText.text = playerConversant.GetText();
            if (!playerConversant.MoveToNextNode())
            {
                nextButton.gameObject.SetActive(false);
            }
        }

        private void HandleNextButtonClicked()
        {
            ShowCurrentText();
        }
    }
}