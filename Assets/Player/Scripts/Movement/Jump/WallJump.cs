using Player.Scripts.States;
using Unity.Mathematics;
using UnityEngine;

public class WallJump : Jump
{
    [SerializeField] private float sideStartPush;
    [SerializeField] private float sideForce;

    private float _sideStartPushSigned;
    private float _sideForceSigned;
    
    protected override void OnActivate()
    {
        SetSide();
        _states.isCanMove = false;
        _rb.linearVelocityX = _sideStartPushSigned;
        // _states.isSlide = PlayerStatesOld.Sides.none; // ?

        base.OnActivate();
    }

    protected override void End()
    {
        base.End();

        _states.isCanMove = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isStarting && _rb.linearVelocityX < sideForce)
        {
            _rb.linearVelocityX = _sideForceSigned;
        }
    }

    protected override void SetForce()
    {
        base.SetForce();

        _rb.linearVelocityX = _sideForceSigned;
    }

    protected override void IsJumping(bool jumping)
    {
        _isJumping = jumping;
        _states.ground = jumping ? Ground.WallJumping : Ground.Falling;
    }

    private void SetSide()
    {
        switch (_states.direction)
        {
            case Direction.Right:
                _sideStartPushSigned = -sideStartPush;
                _sideForceSigned = -sideForce;
                break;
            case Direction.Left:
                _sideStartPushSigned = sideStartPush;
                _sideForceSigned = sideForce;
                break;
        }
    }

    public override void Stop()
    {
        base.Stop();

        _states.isCanMove = true;
    }
}
