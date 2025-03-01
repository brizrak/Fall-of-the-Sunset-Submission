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

        base.StartJump();
    }

    protected override void End()
    {
        base.End();

        states.isCanMove = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isStarting && rb.linearVelocityX < math.abs(sideForceSigned))
        {
            rb.linearVelocityX = sideForceSigned;
        }
    }

    protected override void SetForce()
    {
        base.SetForce();

        rb.linearVelocityX = sideForceSigned;
    }

    protected override void IsJumping(bool jumped)
    {
        isJumping = jumped;
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

    public override void StopJump()
    {
        base.StopJump();

        states.isCanMove = true;
    }
}
