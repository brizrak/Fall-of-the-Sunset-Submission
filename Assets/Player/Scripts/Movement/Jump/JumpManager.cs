using UnityEngine;

[RequireComponent (typeof(PlayerStateOldManagerOld))]
[RequireComponent (typeof(Move))]
[RequireComponent (typeof(Jump), typeof(WallJump), typeof(AirJump))]
public class JumpManager : MonoBehaviour
{
    [SerializeField] private float jumpBuffer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D topCheck;

    private Jump jump;
    private WallJump wallJump;
    private AirJump airJump;
    private PlayerStateOldManagerOld _statesOld;
    private Move move;
    private float timer = 0f;
    private bool isEnded = false;

    private void Awake()
    {
        _statesOld = GetComponent<PlayerStateOldManagerOld>();
        jump = GetComponent<Jump>();
        wallJump = GetComponent<WallJump>();
        airJump = GetComponent<AirJump>();
        move = GetComponent<Move>();
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (_statesOld.isGrounded)
            {
                jump.StartJump();
                timer = -1f;
                if (isEnded)
                {
                    jump.EndJump();
                    isEnded = false;
                }
            }
        }
    }

    public void StartJump()
    {
        if ((_statesOld.isSlide == PlayerStatesOld.Sides.none) && !move.CanWallJump())
        {
            if (_statesOld.isGrounded)
            {
                jump.StartJump();
            }
            else if (_statesOld.isCanAirJump)
            {
                airJump.StartJump();
                _statesOld.isCanAirJump = false;
            }
            else
            {
                timer = jumpBuffer;
            }
        }
        else
        {
            wallJump.StartJump();
        }
    }

    public void EndJump()
    {
        if (_statesOld.isJumped)
        {
            jump.EndJump();
        }
        else if (_statesOld.isAirJumped)
        {
            airJump.EndJump();
        }
        else if (_statesOld.isWallJumped)
        {
            wallJump.EndJump();
        }

        if (timer > 0f)
        {
            isEnded = true;
        }
    }

    public bool IsJumping()
    {
        if (_statesOld.isJumped || _statesOld.isWallJumped || _statesOld.isAirJumped) return true;
        return false;
    }

    public void StopJump()
    {
        jump.StopJump();
        airJump.StopJump();
        wallJump.StopJump();
    }

    private void FixedUpdate()
    {
        if (!IsJumping()) return;

        if (topCheck.IsTouchingLayers(groundLayer) && (_statesOld.isJumped || _statesOld.isWallJumped || _statesOld.isAirJumped))
        {
            StopJump();
        }
    }
}
