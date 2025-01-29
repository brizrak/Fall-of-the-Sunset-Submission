using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Move), typeof(JumpManager))]
public class PlayerController : MonoBehaviour
{
    private InputSystemActions actions;
    private Move playerMove;
    private JumpManager jumpManager;

    private void Awake()
    {
        actions = new InputSystemActions();
        playerMove = GetComponent<Move>();
        jumpManager = GetComponent<JumpManager>();

        actions.Player.Jump.performed += context => jumpManager.StartJump();
        actions.Player.Jump.canceled += context => jumpManager.EndJump();

        actions.Player.Move.performed += context => playerMove.MoveInput(context.ReadValue<Vector2>());
        actions.Player.Move.canceled += context => playerMove.MoveInput(Vector2.zero);

        //Debug
        actions.Player.Stop.performed += context => jumpManager.StopJump();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}
