using UnityEngine;

public class Server : MonoBehaviour {
    [SerializeField]
    private GameObject selectedObject;
    
    private bool active = false;
    
    public static Server CurrentServer;
    
    private void OnCollisionEnter(Collision other) {
        if (!active) return;
        
        if (other.gameObject.CompareTag("Player")) {
            MinigameManager.Instance.OpenWindow();
            CurrentServer = this;
        }
    }

    // activate component of 3 random servers
    public void Activate()
    {
        active = true;
        selectedObject.SetActive(true);
    }

    public void Deactivate() {
        active = false;
        selectedObject.SetActive(false);
    }
}
