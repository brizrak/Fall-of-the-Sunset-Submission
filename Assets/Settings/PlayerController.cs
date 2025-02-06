using UnityEngine;

[RequireComponent(typeof(Move), typeof(JumpManager), typeof(Dash))]
public class PlayerController : MonoBehaviour
{
    private InputSystemActions actions;
    private Move playerMove;
    private JumpManager jumpManager;
    private Dash dash;

    private void Awake()
    {
        actions = new InputSystemActions();
        playerMove = GetComponent<Move>();
        jumpManager = GetComponent<JumpManager>();
        dash = GetComponent<Dash>();

        actions.Player.Jump.performed += context => jumpManager.StartJump();
        actions.Player.Jump.canceled += context => jumpManager.EndJump();

        actions.Player.Move.performed += context => playerMove.MoveInput(context.ReadValue<Vector2>());
        actions.Player.Move.canceled += context => playerMove.MoveInput(Vector2.zero);

        actions.Player.Dash.performed += context => dash.StartDash();

        //Debug
        actions.Player.Stop.performed += context => jumpManager.StopJump();
        actions.Player.Stop.performed += context => dash.StopDash();
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
