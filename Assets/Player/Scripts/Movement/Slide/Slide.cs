using Player.Abilities;
using Player.Scripts.States;
using UnityEngine;

public class Slide : Ability
{
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideBuffer;
    
    private Rigidbody2D _rb;
    private CheckForTouch _checkForTouch;
    private float _bufferTimer;
    private Direction? _sliding;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _checkForTouch = GetComponent<CheckForTouch>();
    }

    protected override void OnActivate()
    {
        _states.ground = Ground.Sliding;
        _sliding = _states.direction;
    }

    private void FixedUpdate()
    {
        SlideBufferTimer();
        if (_sliding is null) return;
        Sliding();
    }

    private void Sliding()
    {
        if (_checkForTouch.IsTouchingWall() is null || _sliding != _states.direction)
        {
            Deactivate();
            Stop();
            _bufferTimer = slideBuffer;
        }
        else if (_states.ground == Ground.Grounded)
        {
            Deactivate();
            Stop();
        }
        else _rb.linearVelocityY = slideSpeed;
    }

    public override void Stop()
    {
        _sliding = null;
        if (_states.ground is not Ground.Grounded && _states.currentAbility is null) _states.ground = Ground.Falling;
    }

    private void SlideBufferTimer()
    {
        if (_bufferTimer > slideBuffer) return;
        if (_bufferTimer > 0)
        {
            _bufferTimer -= Time.fixedDeltaTime;
        }
        else
        {
            _bufferTimer = slideBuffer + 1f;
        }
    }

    public bool CanWallJump()
    {
        return _bufferTimer > 0 && _bufferTimer <= slideBuffer;
    }
}
