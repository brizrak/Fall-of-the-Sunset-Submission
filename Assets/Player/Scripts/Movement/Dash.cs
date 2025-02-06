using UnityEngine;

[RequireComponent(typeof(PlayerStateManager), typeof(Rigidbody2D), typeof(JumpManager))]
public class Dash : MonoBehaviour
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    private Rigidbody2D rb;
    private PlayerStateManager states;
    private JumpManager jumpManager;
    private bool isDashing;
    private float gravityScale;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<PlayerStateManager>();
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

    private void Dashing()
    {
        if (states.viewSide == PlayerStates.Sides.right)
        {
            rb.linearVelocityX = dashSpeed;
        }
        else
        {
            rb.linearVelocityX = -dashSpeed;
        }
    }

    public void StartDash()
    {
        if (!states.dashIsUnlocked || !states.isCanDash) return;
        states.isCanDash = false;
        states.isCanMove = false;
        rb.gravityScale = 0;
        timer = dashTime;
        isDashing = true;
        if (jumpManager.IsJumping()) jumpManager.StopJump();
    }

    private void EndDash()
    {
        states.isCanMove = true;
        rb.gravityScale = gravityScale;
        isDashing = false;
    }

    public void StopDash()
    {
        EndDash();
        timer = -1f;
    }
}
