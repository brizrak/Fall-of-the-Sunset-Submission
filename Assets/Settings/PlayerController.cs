using UnityEngine;

[RequireComponent(typeof(Move), typeof(JumpManager), typeof(Dash))]
public class PlayerController : MonoBehaviour
{
    private InputSystemActions _actions;
    private Move _playerMove;
    private JumpManager _jumpManager;
    private Dash _dash;

    private void Awake()
    {
        _actions = new InputSystemActions();
        _playerMove = GetComponent<Move>();
        _jumpManager = GetComponent<JumpManager>();
        _dash = GetComponent<Dash>();

        _actions.Player.Jump.performed += context => _jumpManager.StartJump();
        _actions.Player.Jump.canceled += context => _jumpManager.EndJump();

        _actions.Player.Move.performed += context => _playerMove.MoveInput(context.ReadValue<Vector2>());
        _actions.Player.Move.canceled += context => _playerMove.MoveInput(Vector2.zero);

        _actions.Player.Dash.performed += context => _dash.TryActivate();

        //Debug
        _actions.Player.Stop.performed += context => _jumpManager.StopJump();
        _actions.Player.Stop.performed += context => _dash.Stop();
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }
}
