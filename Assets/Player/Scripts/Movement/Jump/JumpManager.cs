using UnityEngine;

[RequireComponent (typeof(PlayerStateManager))]
[RequireComponent (typeof(Jump), typeof(WallJump), typeof(AirJump))]
public class JumpManager : MonoBehaviour
{
    private Jump jump;
    private WallJump wallJump;
    private AirJump airJump;
    private PlayerStateManager states;

    [HideInInspector] public bool isCanAirJump = false;

    private void Awake()
    {
        states = GetComponent<PlayerStateManager>();
        jump = GetComponent<Jump>();
        wallJump = GetComponent<WallJump>();
        airJump = GetComponent<AirJump>();
    }

    public void StartJump()
    {
        if (states.isSlide == PlayerStates.Sides.none)
        {
            if (states.isGrounded)
            {
                jump.StartJump();
            }
            else if (isCanAirJump)
            {
                airJump.StartJump();
                isCanAirJump = false;
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
