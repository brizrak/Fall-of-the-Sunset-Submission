using UnityEngine;

public class WallJump : Jump
{
    [SerializeField] private float sideStartPush;
    [SerializeField] private float sideForce;

    public override void StartJump()
    {
        states.isCanMove = false;
        if (states.isSlide == PlayerStates.Sides.right)
        {
            rb.linearVelocityX = -sideStartPush;
        }
        else
        {
            rb.linearVelocityX = sideStartPush;
        }
        states.isSlide = PlayerStates.Sides.none;
        rb.linearVelocityY = startPush;
        isStartJumped = true;
        IsJumped(true);
    }

    protected override void End()
    {
        base.End();

        states.isCanMove = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isStartJumped && rb.linearVelocityX < sideForce)
        {
            rb.linearVelocityX = sideStartPush;
        }
    }

    protected override void SetForce()
    {
        base.SetForce();
        rb.linearVelocityX = sideForce;
    }

    protected override void IsJumped(bool jumped)
    {
        isJumped = jumped;
        states.isWallJumped = jumped;
    }
}
