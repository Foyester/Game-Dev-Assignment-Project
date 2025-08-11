using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool useFixedUpdate = false;

    [Header("Optional Settings")]
    public float boostMultiplier = 2f; // Shift key for faster movement

    [Header("Camera Bounds")]
    public Vector2 topLeft = new Vector2(960.48f, 640.48f);
    public Vector2 bottomRight = new Vector2(1992.723f, -168.514f);

    private float fixedZ; // store the constant Z position

    void Start()
    {
        fixedZ = transform.position.z; // Remember starting Z so it never changes
    }

    void Update()
    {
        if (!useFixedUpdate)
            HandleMovement();
    }

    void FixedUpdate()
    {
        if (useFixedUpdate)
            HandleMovement();
    }

    void HandleMovement()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * boostMultiplier : moveSpeed;

        // Movement in X & Y only
        Vector3 direction = new Vector3(
            Input.GetAxisRaw("Horizontal"), // A/D
            Input.GetAxisRaw("Vertical"),   // W/S
            0f                              // No Z movement
        );

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // Clamp position to bounds and lock Z
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, topLeft.x, bottomRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomRight.y, topLeft.y);
        pos.z = fixedZ; // Keep Z locked
        transform.position = pos;
    }
}



