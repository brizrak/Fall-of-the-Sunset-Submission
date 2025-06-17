using UnityEngine;
using Player.States;
using Player.Abilities;
using Animations;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jump : JumpAbility
    {
        [Header("Settings")]
        [SerializeField] protected float startPush;
        [SerializeField] private float force;
        [SerializeField] private float time;
        [SerializeField] private float endPush;
        [SerializeField] private float stopPush;
        [Header("Animations")]
        [SerializeField] private AnimationPreset holdAnimation;
        [SerializeField] private AnimationPreset endAnimation;

        protected Rigidbody2D _rb;

        private float _timer;
        protected bool _isStarting = false;
        private bool _isEndPushing = false;
        protected bool _isJumping = false;
        private bool _isEnding = false;

        protected override void IsJumping(bool jumping)
        {
            _isJumping = jumping;
            _states.ground = jumping ? Ground.Jumping : Ground.Falling;
        }

        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            
            _rb.linearVelocityY = startPush;
            _isStarting = true;
        }

        public void EndJump()
        {
            if (_isStarting)
            {
                _isEnding = true;
                return;
            }

            if (_isJumping && !_isStarting && !_isEndPushing)
            {
                End();
            }
        }

        protected virtual void End()
        {
            _rb.linearVelocityY = endPush;
            _isEndPushing = true;
            _isEnding = false;
            _animationController.HandleAnimation(endAnimation);
        }

        protected virtual void FixedUpdate()
        {
            if (!_isJumping) return;

            if (_isStarting)
            {
                if (_rb.linearVelocityY < force)
                {
                    _isStarting = false;
                    if (_isEnding)
                    {
                        EndJump();
                    }
                    else
                    {
                        _timer = time;
                        _animationController.HandleAnimation(holdAnimation);
                    }

                }
            }

            if (_isJumping && !_isStarting && !_isEndPushing)
            {
                if (_timer > 0)
                {
                    SetForce();
                    _timer -= Time.fixedDeltaTime;
                }
                else
                {
                    EndJump();
                }
            }

            if (_rb.linearVelocityY < 0 && _isJumping)
            {
                IsJumping(false);
                _isEndPushing = false;
            }
        }

        protected virtual void SetForce()
        {
            _rb.linearVelocityY = force;
        }

        public override void Stop()
        {
            _rb.linearVelocityY = stopPush;
            _isStarting = false;
            _isEndPushing = false;
            _isEnding = false;
            IsJumping(false);
        }
    }
}