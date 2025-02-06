using UnityEngine;

[RequireComponent (typeof(PlayerStateManager))]
[RequireComponent (typeof(Move))]
[RequireComponent (typeof(Jump), typeof(WallJump), typeof(AirJump))]
public class JumpManager : MonoBehaviour
{
    [SerializeField] private float jumpBuffer;

    private Jump jump;
    private WallJump wallJump;
    private AirJump airJump;
    private PlayerStateManager states;
    private Move move;
    private float timer = 0f;
    private bool isEnded = false;

    private void Awake()
    {
        states = GetComponent<PlayerStateManager>();
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
            if (states.isGrounded)
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
        if ((states.isSlide == PlayerStates.Sides.none) && !move.CanWallJump())
        {
            if (states.isGrounded)
            {
                jump.StartJump();
            }
            else if (states.isCanAirJump)
            {
                airJump.StartJump();
                states.isCanAirJump = false;
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
        if (states.isJumped)
        {
            jump.EndJump();
        }
        else if (states.isAirJumped)
        {
            airJump.EndJump();
        }
        else {
            wallJump.EndJump();
        }

        if (timer > 0f)
        {
            isEnded = true;
        }
    }

    public bool IsJumping()
    {
        if (states.isJumped || states.isWallJumped || states.isAirJumped) return true;
        return false;
    }

    public void StopJump()
    {
        jump.StopJump();
        airJump.StopJump();
        wallJump.StopJump();
    }

    //Задача17
    //
    //[SerializeField] private LayerMask groundLayer;
    //[SerializeField] private Collider2D topCheck;
    //
    //private void FixedUpdate()
    //{
    //    if (topCheck.IsTouchingLayers(groundLayer) && (states.isJumped || states.isWallJumped || states.isAirJumped))
    //    {
    //        StopJump();
    //    }
    //}
}
