using UnityEngine;

public class Server : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player")) {
            MinigameManager.Instance.OpenWindow();
        }
    }
}
