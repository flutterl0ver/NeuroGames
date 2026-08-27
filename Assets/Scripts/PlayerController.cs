using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Movement speed in units per second")]
    public float speed = 5f;

    private static bool isMovementLocked;
    private Rigidbody rb;

    [Tooltip("Optional: assign the child GameObject that represents the player's visual body. Only this will rotate.")]
    public Transform body;

    [Tooltip("Rotation speed (higher = faster)")]
    public float rotationSpeed = 10f;

    public static bool IsMovementLocked => isMovementLocked;

    public static void SetMovementLocked(bool locked)
    {
        isMovementLocked = locked;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Prevent the cube root from rotating via physics; rotate visual body separately
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

        Vector3 input = new Vector3(h, 0f, v);
        Vector3 move = input.normalized * speed;
        rb.linearVelocity = move;

        // Rotate visual body toward movement direction (Y axis only)
        if (input.sqrMagnitude > 0.001f)
        {
            Vector3 dir = input.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            // keep only Y rotation
            Quaternion targetY = Quaternion.Euler(0f, targetRot.eulerAngles.y, 0f);

            if (body == null)
            {
                // try to auto-find a child named 'body' or 'Body' if not assigned
                Transform found = transform.Find("body") ?? transform.Find("Body");
                if (found != null) body = found;
            }

            if (body != null)
            {
                body.rotation = Quaternion.Slerp(body.rotation, targetY, rotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                // fallback: rotate the root (not recommended if using physics), keep Y only
                transform.rotation = Quaternion.Slerp(transform.rotation, targetY, rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
