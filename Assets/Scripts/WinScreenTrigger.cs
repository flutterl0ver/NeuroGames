using UnityEngine;

public class WinScreenTrigger : MonoBehaviour
{
    public static WinScreenTrigger singleton;
    public static bool allCardsGathered = false;
    public GameObject winScreen;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnce = true;

    private bool alreadyTriggered;

    private void Start()
    {
        winScreen.SetActive(false);
        singleton = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!allCardsGathered) return;

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

        winScreen.SetActive(true);
    }
}
