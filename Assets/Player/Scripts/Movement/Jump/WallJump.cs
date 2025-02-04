using Unity.Mathematics;
using UnityEngine;

public class WallJump : Jump
{
    [SerializeField] private float sideStartPush;
    [SerializeField] private float sideForce;

    private float sideStartPushSigned;
    private float sideForceSigned;

    public override void StartJump()
    {
        SetSide();
        states.isCanMove = false;
        rb.linearVelocityX = sideStartPushSigned;
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

        if (isStartJumped && rb.linearVelocityX < math.abs(sideForceSigned))
        {
            rb.linearVelocityX = sideForceSigned;
        }
    }

    protected override void SetForce()
    {
        base.SetForce();

        rb.linearVelocityX = sideForceSigned;
    }

    protected override void IsJumped(bool jumped)
    {
        isJumped = jumped;
        states.isWallJumped = jumped;
    }

    private void SetSide()
    {
        if (states.isSlide == PlayerStates.Sides.right)
        {
            sideStartPushSigned = -sideStartPush;
            sideForceSigned = -sideForce;
        }
        else if (states.isSlide == PlayerStates.Sides.left)
        {
            sideStartPushSigned = sideStartPush;
            sideForceSigned = sideForce;
        }
    }
}
