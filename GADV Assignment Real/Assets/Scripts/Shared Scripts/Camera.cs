///simple camera script that allows me to move it up and dow, left and right etc. it has boost function using shift and locks it'z z position since it's a 2d game.
///THere is also boundries set by direct coordinates in the toppleft and bottom right. i could've done better or used a more adaptable code but i didnt care and 
///just hardcoded the scene since moving th tilemap to fit was easier.

using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool useFixedUpdate = false;

    [Header("Optional Settings")]
    public float boostMultiplier = 2f; 

    [Header("Camera Bounds")]
    public Vector2 topLeft = new Vector2(960.48f, 640.48f);
    public Vector2 bottomRight = new Vector2(1992.723f, -168.514f);

    private float fixedZ; 

    void Start()
    {
        fixedZ = transform.position.z; 
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
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"),   
            0f                             
        );

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, topLeft.x, bottomRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomRight.y, topLeft.y);
        pos.z = fixedZ; 
        transform.position = pos;
    }
}



