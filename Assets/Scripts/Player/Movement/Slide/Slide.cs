using UnityEngine;
using States;
using Abilities;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CheckForTouch))]
    public class Slide : Ability
    {
        [SerializeField] private float slideSpeed;
        [SerializeField] private float wallJumpBuffer;

        [HideInInspector] public Direction direction;

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
            base.OnActivate();
            
            _states.ground = Ground.Sliding;
            _sliding = _states.direction;
            direction = _states.direction;
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
                _bufferTimer = wallJumpBuffer;
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
            if (_states.ground is not Ground.Grounded && _states.currentAbility is null)
                _states.ground = Ground.Falling;
        }

        private void SlideBufferTimer()
        {
            if (_bufferTimer > wallJumpBuffer) return;
            if (_bufferTimer > 0 && _states.ground is not Ground.Grounded)
            {
                _bufferTimer -= Time.fixedDeltaTime;
            }
            else
            {
                _bufferTimer = wallJumpBuffer + 1f;
            }
        }

        public bool CanWallJump()
        {
            return _sliding is not null || (_bufferTimer > 0 && _bufferTimer <= wallJumpBuffer);
        }
    }
}