using Player.Scripts.States;
using UnityEngine;

[RequireComponent (typeof(PlayerStates), typeof(Move))]
[RequireComponent (typeof(Jump), typeof(WallJump), typeof(AirJump))]
public class JumpManager : MonoBehaviour
{
    [SerializeField] private float jumpBuffer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D topCheck;

    private Jump _jump;
    private WallJump _wallJump;
    private AirJump _airJump;
    private PlayerStates _states;
    private Move _move;
    private float _timer = 0f;
    private bool _isEnded = false;

    private void Awake()
    {
        _states = GetComponent<PlayerStates>();
        _jump = GetComponent<Jump>();
        _wallJump = GetComponent<WallJump>();
        _airJump = GetComponent<AirJump>();
        _move = GetComponent<Move>();
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
        if (/*_states.isSlide == PlayerStatesOld.Sides.none && !_move.CanWallJump()*/true) // add sliding
        {
            switch (_states.ground)
            {
                case Ground.Grounded or Ground.CoyoteTime:
                    _jump.StartJump();
                    break;
                case Ground.Falling:
                    _airJump.StartJump();
                    break;
                default:
                    _timer = jumpBuffer;
                    break;
            }
        }
        else  _wallJump.StartJump();
    }

    public void EndJump()
    {
        switch (_states.ground)
        {
            case Ground.Jumping:
                _jump.EndJump();
                break;
            // else if (_states.isAirJumped) _airJump.EndJump(); изменить если будет оставлен air jump
            case Ground.WallJumping:
                _wallJump.EndJump();
                break;
        }

        if (_timer > 0f) _isEnded = true;
    }

    public bool IsJumping() => _states.ground is (Ground.Jumping or Ground.WallJumping);

    public void StopJump()
    {
        _jump.StopJump();
        _airJump.StopJump();
        _wallJump.StopJump();
    }

    private void FixedUpdate()
    {
        if (!IsJumping()) return;

        if (topCheck.IsTouchingLayers(groundLayer)) StopJump();
    }
}
