using Unity.Mathematics;
using UnityEngine;

public class WallJump : Jump
{
    [SerializeField] private float sideStartPush;
    [SerializeField] private float sideForce;

    private float sideStartPushSigned;
    private float sideForceSigned;
    private PlayerStateOldManagerOld StatesOld;

    protected void Start()
    {
        StatesOld = GetComponent<PlayerStateOldManagerOld>();
    }
    
    protected override void OnActivate()
    {
        SetSide();
        StatesOld.isCanMove = false;
        _rb.linearVelocityX = sideStartPushSigned;
        StatesOld.isSlide = PlayerStatesOld.Sides.none;

        base.OnActivate();
    }

    protected override void End()
    {
        base.End();

        StatesOld.isCanMove = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isStarting && _rb.linearVelocityX < math.abs(sideForceSigned))
        {
            _rb.linearVelocityX = sideForceSigned;
        }
    }

    protected override void SetForce()
    {
        base.SetForce();

        _rb.linearVelocityX = sideForceSigned;
    }

    protected override void IsJumping(bool jumped)
    {
        _isJumping = jumped;
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
