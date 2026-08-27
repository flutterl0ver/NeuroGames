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
                GameObject managerObject = new GameObject("DialogueManager");
                _instance = managerObject.AddComponent<DialogueManager>();
            }

            return _instance;
        }
    }

    private Canvas canvas;
    private GameObject dialoguePanel;
    private Text dialogueText;
    private Button closeButton;
    private bool isVisible;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        CreateUi();
    }

    private void CreateUi()
    {
        GameObject canvasObject = GameObject.Find("DialogueCanvas");
        if (canvasObject == null)
        {
            canvasObject = new GameObject("DialogueCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }
        else
        {
            canvas = canvasObject.GetComponent<Canvas>();
        }

        if (canvas == null)
        {
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        GameObject panelObject = canvasObject.transform.Find("DialoguePanel")?.gameObject;
        if (panelObject == null)
        {
            panelObject = new GameObject("DialoguePanel");
            panelObject.transform.SetParent(canvasObject.transform, false);

            RectTransform panelRect = panelObject.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.15f);
            panelRect.anchorMax = new Vector2(0.5f, 0.15f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(600f, 180f);
            panelRect.anchoredPosition = Vector2.zero;

            Image panelImage = panelObject.AddComponent<Image>();
            panelImage.color = new Color(0.05f, 0.05f, 0.05f, 0.8f);

            GameObject textObject = new GameObject("DialogueText");
            textObject.transform.SetParent(panelObject.transform, false);

            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0f, 0f);
            textRect.anchorMax = new Vector2(1f, 1f);
            textRect.offsetMin = new Vector2(20f, 20f);
            textRect.offsetMax = new Vector2(-20f, -20f);

            dialogueText = textObject.AddComponent<Text>();
            dialogueText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            dialogueText.fontSize = 28;
            dialogueText.alignment = TextAnchor.MiddleCenter;
            dialogueText.color = Color.white;
            dialogueText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dialogueText.verticalOverflow = VerticalWrapMode.Truncate;

            GameObject buttonObject = new GameObject("CloseButton");
            buttonObject.transform.SetParent(panelObject.transform, false);

            RectTransform buttonRect = buttonObject.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0f);
            buttonRect.anchorMax = new Vector2(0.5f, 0f);
            buttonRect.sizeDelta = new Vector2(150f, 40f);
            buttonRect.anchoredPosition = new Vector2(0f, 25f);

            Image buttonImage = buttonObject.AddComponent<Image>();
            buttonImage.color = new Color(0.85f, 0.85f, 0.85f, 1f);

            closeButton = buttonObject.AddComponent<Button>();
            closeButton.onClick.AddListener(HideDialogue);

            GameObject buttonTextObject = new GameObject("ButtonText");
            buttonTextObject.transform.SetParent(buttonObject.transform, false);

            RectTransform buttonTextRect = buttonTextObject.AddComponent<RectTransform>();
            buttonTextRect.anchorMin = new Vector2(0f, 0f);
            buttonTextRect.anchorMax = new Vector2(1f, 1f);
            buttonTextRect.offsetMin = Vector2.zero;
            buttonTextRect.offsetMax = Vector2.zero;

            Text buttonText = buttonTextObject.AddComponent<Text>();
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.text = "Закрыть";
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.fontSize = 22;
            buttonText.color = Color.black;
        }

        dialoguePanel = panelObject;
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<Text>();
        closeButton = dialoguePanel.transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(HideDialogue);
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string message)
    {
        if (dialogueText == null)
        {
            CreateUi();
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
