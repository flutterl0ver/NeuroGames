using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject targetWindow;

    private static MinigameManager _instance;

    public static MinigameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<MinigameManager>();
            }

            if (_instance == null)
            {
                GameObject go = new GameObject("MinigameManager");
                _instance = go.AddComponent<MinigameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void OpenWindow()
    {
        if (targetWindow == null)
        {
            Debug.LogWarning("MinigameManager: targetWindow не назначен в Inspector.");
            return;
        }

        targetWindow.SetActive(true);
        PlayerController.SetMovementLocked(true);
    }

    public void CloseWindow()
    {
        if (targetWindow == null)
            return;

        targetWindow.SetActive(false);
        PlayerController.SetMovementLocked(false);
    }

    public bool IsWindowOpen()
    {
        return targetWindow != null && targetWindow.activeSelf;
    }
}
