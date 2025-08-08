using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool useFixedUpdate = false;

    [Header("Optional Settings")]
    public float boostMultiplier = 2f; // Shift key for faster movement
    public bool constrainToXZ = true;  // Set false if you want Y movement too

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

        Vector3 direction = new Vector3(
            Input.GetAxisRaw("Horizontal"), // A/D
            0f,
            Input.GetAxisRaw("Vertical")    // W/S
        );

        if (!constrainToXZ)
            direction.y = Input.GetAxisRaw("Jump"); // Optional Y movement (e.g. Space/Control)

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}

