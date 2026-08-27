using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;

    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DialogueManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button closeButton;

    private bool isVisible;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(HideDialogue);
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void ShowDialogue(string message)
    {
        if (dialogueText == null || dialoguePanel == null)
        {
            Debug.LogWarning("DialogueManager: не назначены serializeField ссылки на Text/Panel.");
            return;
        }

        dialogueText.text = message;
        dialoguePanel.SetActive(true);
        isVisible = true;
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        isVisible = false;
    }

    private void Update()
    {
        if (isVisible && Input.GetKeyDown(KeyCode.Escape))
        {
            HideDialogue();
        }
    }

    public bool IsVisible => isVisible;
}
