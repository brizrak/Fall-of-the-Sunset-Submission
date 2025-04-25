using Player.Abilities;
using Player.Scripts.States;
using UnityEngine;

[RequireComponent(typeof(PlayerStates), typeof(Rigidbody2D), typeof(JumpManager))]
public class Dash : Ability
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    private Rigidbody2D rb;
    private JumpManager jumpManager;
    private bool isDashing;
    private float gravityScale;
    private float timer;

    protected override void Awake()
    {
        base.Awake();
        
        rb = GetComponent<Rigidbody2D>();
        jumpManager = GetComponent<JumpManager>();
        gravityScale = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        if (!isDashing) return;
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            rb.linearVelocityY = 0;
            Dashing();
        }
        else
        {
            EndDash();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Dashing()
    {
        rb.linearVelocityX = _states.direction switch
        {
            ViewDirection.Right => dashSpeed,
            ViewDirection.Left => -dashSpeed,
            _ => rb.linearVelocityX
        };
    }

    private void StartDash()
    {
        // if (!_states.dashIsUnlocked || !_states.isCanDash) return;
        // _states.isCanDash = false;
        _states.isCanMove = false;
        rb.gravityScale = 0;
        timer = dashTime;
        isDashing = true;
        if (jumpManager.IsJumping()) jumpManager.StopJump();
    }

    private void EndDash()
    {
        _states.isCanMove = true;
        rb.gravityScale = gravityScale;
        isDashing = false;
        Deactivate();
    }

    public void StopDash()
    {
        EndDash();
        timer = -1f;
    }

    protected override void OnActivate()
    {
        StartDash();
    }

    public override void Stop()
    {
        StopDash();
    }
}
