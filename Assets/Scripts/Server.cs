using UnityEngine;

public class Server : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            MinigameManager.Instance.OpenWindow();
        }
    }

    // activate component of 3 random servers
    public void Activate()
    {
        var collider = GetComponent<BoxCollider>();
        collider.enabled = true;
    }
}
