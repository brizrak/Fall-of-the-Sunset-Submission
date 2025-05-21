using Player.Abilities;
using Player.Scripts.States;
using UnityEngine;

[RequireComponent(typeof(PlayerStates), typeof(Rigidbody2D), typeof(JumpManager))]
public class Dash : Ability
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    private Rigidbody2D _rb;
    private JumpManager _jumpManager;
    private bool _isDashing;
    private float _gravityScale;
    private float _timer;

    protected override void Awake()
    {
        base.Awake();
        
        _rb = GetComponent<Rigidbody2D>();
        _jumpManager = GetComponent<JumpManager>();
        _gravityScale = _rb.gravityScale;
    }

    private void FixedUpdate()
    {
        if (!_isDashing) return;
        if (_timer > 0)
        {
            _timer -= Time.fixedDeltaTime;
            _rb.linearVelocityY = 0;
            Dashing();
        }
        else EndDash();
    }
    
    private void Dashing()
    {
        _rb.linearVelocityX = _states.direction switch
        {
            Direction.Right => dashSpeed,
            Direction.Left => -dashSpeed,
            _ => _rb.linearVelocityX
        };
    }

    protected override void OnActivate()
    {
        _states.isCanMove = false;
        _rb.gravityScale = 0;
        _timer = dashTime;
        _isDashing = true;
    }

    private void EndDash()
    {
        _states.isCanMove = true;
        _rb.gravityScale = _gravityScale;
        _isDashing = false;
        Deactivate();
    }

    public override void Stop()
    {
        EndDash();
        _timer = -1f;
    }
}
