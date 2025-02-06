using UnityEngine;

public class PlayerStateManager : PlayerStates
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime;
    [SerializeField] private Collider2D bottomCheck;

    private float coyoteTimeCounter;

    private void Awake()
    {
        isSlide = Sides.none;
    }

    private void Update()
    {
        if (isJumped)
        {
            isGrounded = false;
        }

        if (isGrounded || isSlide != Sides.none)
        {
            if (dashIsUnlocked)
            {
                isCanDash = true;
            }
            if (airJumpIsUnlocked)
            {
                isCanAirJump = true;
            }
        }

        CoyoteTime();
    }

    private void CoyoteTime()
    {
        if (!bottomCheck.IsTouchingLayers(groundLayer))
        {
            //if (leftBotCheck.IsTouchingLayers(groundLayer) || rightBotCheck.IsTouchingLayers()) return;

            if (coyoteTimeCounter > coyoteTime)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
            else
            {
                coyoteTimeCounter = coyoteTime + 1f;
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = true;
            if (coyoteTimeCounter <= coyoteTime)
            {
                coyoteTimeCounter = coyoteTime + 1f;
            }
        }
    }

    public void CanAirJump()
    {
        airJumpIsUnlocked = true;
        isCanAirJump = true;
    }

    public void CanSlide()
    {
        slideIsUnlocked = true;
    }

    public void CanDash()
    {
        dashIsUnlocked = true;
        isCanDash = true;
    }
}
