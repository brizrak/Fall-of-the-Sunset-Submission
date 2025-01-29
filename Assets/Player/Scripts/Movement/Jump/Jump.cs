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
    protected bool isStartJumped = false;
    private bool isEndJumped = false;
    protected bool isJumped = false;
    private bool isStoped = false;

    protected virtual void IsJumped(bool jumped)
    {
        isJumped = jumped;
        states.isJumped = jumped;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<PlayerStateManager>();
    }

    public virtual void StartJump()
    {
        rb.linearVelocityY = startPush;
        isStartJumped = true;
        IsJumped(true);
    }

    public void EndJump()
    {
        if (isStartJumped)
        {
            isStoped = true;
            return;
        }
        if (isJumped && !isStartJumped && !isEndJumped)
        {
            End();
        }
    }

    protected virtual void End()
    {
        rb.linearVelocityY = endPush;
        isEndJumped = true;
        isStoped = false;
    }

    protected virtual void FixedUpdate()
    {
        if (rb.linearVelocityY <= 0 && isJumped)
        {
            IsJumped(false);
            isEndJumped = false;
        }

        if (isStartJumped)
        {
            if (rb.linearVelocityY < force)
            {
                isStartJumped = false;
                if (isStoped)
                {
                    EndJump();
                }
                else
                {
                    timer = time;
                }
                
            }
        }

        if (isJumped && !isStartJumped && !isEndJumped)
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
    }

    protected virtual void SetForce()
    {
        rb.linearVelocityY = force;
    }

    public void StopJump()
    {
        rb.linearVelocityY = stopPush;
        isStartJumped = false;
        isEndJumped = false;
        isStoped = false;
        IsJumped(false);
    }
}