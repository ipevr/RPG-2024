using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;

namespace RPG.UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private Button choiceButtonPrefab;
        [SerializeField] private Transform aiResponse;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private TextMeshProUGUI aiText;
        
        private PlayerConversant playerConversant;
        private DialogueNode currentNode;

        private void Awake()
        {
            playerConversant = PlayerConversant.GetPlayerConversant();
        }

        private void OnEnable()
        {
            nextButton.onClick.AddListener(playerConversant.Next);
            quitButton.onClick.AddListener(playerConversant.Quit);
        }

        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(playerConversant.Next);
            quitButton.onClick.RemoveListener(playerConversant.Quit);
        }

        private void Start()
        {
            playerConversant.OnConversationUpdated += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            
            if (!playerConversant.IsActive())
            {
                return;
            }
            
            var isChoosing = playerConversant.IsChoosing;
            aiResponse.gameObject.SetActive(!isChoosing);
            choiceRoot.gameObject.SetActive(isChoosing);
            
            if (isChoosing)
            {
                BuildChoiceList();
            }
            else
            {
                aiText.text = playerConversant.CurrentNode.Text;
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            DestroyChoiceButtons();
            
            foreach (var choice in playerConversant.GetChoices())
            {
                var button = Instantiate(choiceButtonPrefab, choiceRoot);
                button.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
                button.onClick.AddListener(() => playerConversant.SelectChoice(choice));
            }
        }

        private void DestroyChoiceButtons()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
        }
    }
}