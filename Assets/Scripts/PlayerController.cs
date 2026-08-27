using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Movement speed in units per second")]
    public float speed = 5f;

    private static bool isMovementLocked;
    private Rigidbody rb;

    public static bool IsMovementLocked => isMovementLocked;

    public static void SetMovementLocked(bool locked)
    {
        isMovementLocked = locked;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Prevent the cube from tipping over when colliding
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (isMovementLocked)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Horizontal: A/D or Left/Right. Vertical: W/S or Up/Down.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(h, 0f, v).normalized * speed;
        rb.linearVelocity = move;
    }
}
