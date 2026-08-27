using UnityEngine;

public class Server : MonoBehaviour
{
    // activate component of 3 random servers
    public void Activate()
    {
        var collider = GetComponent<BoxCollider>();
        collider.enabled = true;
    }
}
