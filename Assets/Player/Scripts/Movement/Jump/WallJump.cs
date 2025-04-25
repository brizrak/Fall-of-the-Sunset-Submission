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
        StatesOld.isCanMove = false;
        rb.linearVelocityX = sideStartPushSigned;
        StatesOld.isSlide = PlayerStatesOld.Sides.none;

        base.StartJump();
    }

    protected override void End()
    {
        base.End();

        StatesOld.isCanMove = true;
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
        StatesOld.isWallJumped = jumped;
    }

    private void SetSide()
    {
        if (StatesOld.isSlide == PlayerStatesOld.Sides.right)
        {
            sideStartPushSigned = -sideStartPush;
            sideForceSigned = -sideForce;
        }
        else if (StatesOld.isSlide == PlayerStatesOld.Sides.left)
        {
            sideStartPushSigned = sideStartPush;
            sideForceSigned = sideForce;
        }
    }

    public override void StopJump()
    {
        base.StopJump();

        StatesOld.isCanMove = true;
    }
}
