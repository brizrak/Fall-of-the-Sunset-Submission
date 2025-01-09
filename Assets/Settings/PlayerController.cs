using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Move))]
public class PlayerController : MonoBehaviour
{
    private InputSystemActions actions;
    private Move playerMove;
    
    private void Awake()
    {
        actions = new InputSystemActions();
        playerMove = GetComponent<Move>();

        actions.Player.Jump.performed += context => OnJump();

        actions.Player.Move.performed += context => playerMove.MoveInput(context.ReadValue<Vector2>());
        actions.Player.Move.canceled += context => playerMove.MoveInput(Vector2.zero);
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void OnJump() { }
}
