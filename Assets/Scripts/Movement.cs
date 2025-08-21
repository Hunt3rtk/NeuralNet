using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

public class Movement : MonoBehaviour
{

    private Vector2 moveDirection;

    private Vector2 rotateDirection;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float rotationSpeed = 50f;

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
        transform.Rotate(0, 0, rotateDirection.y * rotationSpeed * Time.deltaTime);
    }
    public void InputMove(CallbackContext ctx)
    {
        // Move the GameObject in the specified direction
        Move(ctx.ReadValue<Vector2>());
    }

    public void Move(Vector2 direction)
    {
        moveDirection = new Vector2(direction.y, 0);
        rotateDirection = new Vector2(0, -direction.x);
    }
}
