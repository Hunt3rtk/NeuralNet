using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

public class Movement : MonoBehaviour
{

    private Vector2 moveDirection;

    [SerializeField]
    private float speed = 5f;

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
    public void InputMove(CallbackContext ctx)
    {
        // Move the GameObject in the specified direction
        Move(ctx.ReadValue<Vector2>());
    }

    public void Move(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }
}
