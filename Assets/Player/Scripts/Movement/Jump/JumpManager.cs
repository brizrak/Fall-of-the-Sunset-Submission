using UnityEngine;
using Player.States;

namespace Player.Movement
{
    [RequireComponent(typeof(PlayerStates), typeof(Slide))]
    [RequireComponent(typeof(Jump), typeof(WallJump))]
    public class JumpManager : MonoBehaviour
    {
        [SerializeField] private float jumpBuffer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Collider2D topCheck;

        private Jump _jump;
        private WallJump _wallJump;
        private PlayerStates _states;
        private Slide _slide;
        private float _timer = 0f;
        private bool _isEnded = false;

        private void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _jump = GetComponent<Jump>();
            _wallJump = GetComponent<WallJump>();
            _slide = GetComponent<Slide>();
        }

        private void Update()
        {
            if (_timer <= 0f) return;
            _timer -= Time.deltaTime;
            if (_states.ground is not (Ground.Grounded or Ground.CoyoteTime)) return;
            StartJump();
            _timer = -1f;
            if (!_isEnded) return;
            EndJump();
            _isEnded = false;
        }

        public void StartJump()
        {
            if (_states.ground is not Ground.Sliding && !_slide.CanWallJump())
            {
                if (_states.ground is (Ground.Grounded or Ground.CoyoteTime)) _jump.TryActivate();
                else _timer = jumpBuffer;
            }
            else _wallJump.TryActivate();
        }

        public void EndJump()
        {
            switch (_states.ground)
            {
                case Ground.Jumping:
                    _jump.EndJump();
                    break;
                case Ground.WallJumping:
                    _wallJump.EndJump();
                    break;
            }

            if (_timer > 0f) _isEnded = true;
        }

        public bool IsJumping() => _states.ground is (Ground.Jumping or Ground.WallJumping);

        public void StopJump()
        {
            switch (_states.ground)
            {
                case Ground.Jumping:
                    _jump.Stop();
                    break;
                case Ground.WallJumping:
                    _wallJump.Stop();
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (!IsJumping()) return;

            if (topCheck.IsTouchingLayers(groundLayer)) StopJump();
        }

        public int? GetPriority()
        {
            return _states.ground switch
            {
                Ground.Jumping => _jump.priority,
                Ground.WallJumping => _wallJump.priority,
                _ => null
            };
        }
    }
}