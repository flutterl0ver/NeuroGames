using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 5)]
    [SerializeField] private string dialogueText = "Привет! Я здесь, чтобы рассказать тебе историю.";

    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private string playerTag = "Player";

    private bool alreadyTriggered;

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag(playerTag)
            || other.GetComponent<PlayerController>() != null
            || other.GetComponentInParent<PlayerController>() != null;
        if (!isPlayer)
        {
            return;
        }

        if (triggerOnce && alreadyTriggered)
        {
            return;
        }

        alreadyTriggered = true;

        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("DialogueTrigger: не найден DialogueManager в сцене. Добавьте существующий объект с компонентом DialogueManager и назначьте ссылки через Inspector.");
            return;
        }

        DialogueManager.Instance.ShowDialogue(dialogueText);
    }
}
