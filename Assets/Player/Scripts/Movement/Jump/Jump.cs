using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStateManager))]
public class Jump : MonoBehaviour
{
    [SerializeField] protected float startPush;
    [SerializeField] private float force;
    [SerializeField] private float time;
    [SerializeField] private float endPush;
    [SerializeField] private float stopPush;

    protected Rigidbody2D rb;
    protected PlayerStateManager states;

    private float timer;
    protected bool isStarting = false;
    private bool isEndPushing = false;
    protected bool isJumping = false;
    private bool isEnding = false;

    protected virtual void IsJumping(bool jumping)
    {
        isJumping = jumping;
        states.isJumped = jumping;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<PlayerStateManager>();
    }


    public virtual void StartJump()
    {
        rb.linearVelocityY = startPush;
        isStarting = true;
        IsJumping(true);
    }

    public void EndJump()
    {
        if (isStarting)
        {
            isEnding = true;
            return;
        }
        if (isJumping && !isStarting && !isEndPushing)
        {
            End();
        }
    }

    protected virtual void End()
    {
        rb.linearVelocityY = endPush;
        isEndPushing = true;
        isEnding = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!isJumping) return;

        if (isStarting)
        {
            if (rb.linearVelocityY < force)
            {
                isStarting = false;
                if (isEnding)
                {
                    EndJump();
                }
                else
                {
                    timer = time;
                }
                
            }
        }

        if (isJumping && !isStarting && !isEndPushing)
        {
            if (timer > 0)
            {
                SetForce();
                timer -= Time.fixedDeltaTime;
            }
            else
            {
                EndJump();
            }
        }

        if (rb.linearVelocityY <= 0 && isJumping)
        {
            IsJumping(false);
            isEndPushing = false;
        }
    }

    protected virtual void SetForce()
    {
        rb.linearVelocityY = force;
    }

    public virtual void StopJump()
    {
        rb.linearVelocityY = stopPush;
        isStarting = false;
        isEndPushing = false;
        isEnding = false;
        IsJumping(false);
    }
}