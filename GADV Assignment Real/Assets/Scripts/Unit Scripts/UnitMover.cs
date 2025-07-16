using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int movementRange = 3;
    private bool canMove = false;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (canMove && Input.GetMouseButtonDown(1)) // Right-click to move
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (Vector3.Distance(transform.position, mousePos) <= movementRange)
            {
                targetPosition = mousePos;
                Debug.Log("Moving to " + targetPosition);
            }
        }

        // Smooth movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }
}

