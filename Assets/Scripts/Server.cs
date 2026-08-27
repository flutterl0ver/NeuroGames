using UnityEngine;

public class Server : MonoBehaviour
{
    private bool active = false;
    
    private void OnCollisionEnter(Collision other) {
        if (!active) return;
        
        if (other.gameObject.CompareTag("Player")) {
            MinigameManager.Instance.OpenWindow();
        }
    }

    // activate component of 3 random servers
    public void Activate()
    {
        active = true;
    }
}
