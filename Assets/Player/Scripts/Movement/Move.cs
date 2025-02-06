using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStateManager))]
public class Move : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decereration;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideBuffer;

    [Header("Checkers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D leftBotCheck;
    [SerializeField] private Collider2D leftTopCheck;
    [SerializeField] private Collider2D rightBotCheck;
    [SerializeField] private Collider2D rightTopCheck;

    private Rigidbody2D rb;
    private PlayerStateManager states;
    private float currentSpeed = 0;
    private float bufferTimer;
    private Vector2 moveInput;
    public void MoveInput(Vector2 moveInput) { this.moveInput = moveInput; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<PlayerStateManager>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        MaxFallSpeed();
        Slide();
        MoveSide();
        SlideBufferTimer();
    }

    private void PlayerMove()
    {
        if (!states.isCanMove) {
            currentSpeed = 0;
            return;
        }

        if (moveInput.x != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveInput.x, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, decereration * Time.fixedDeltaTime);
        }

        rb.linearVelocityX = moveInput.x * moveSpeed;
    }

    public void MoveSide()
    {
        if (!states.isCanMove) return;

        if (moveInput.x < 0)
        {
            states.viewSide = PlayerStates.Sides.left;
        }
        else if (moveInput.x > 0)
        {
            states.viewSide = PlayerStates.Sides.right;
        }
    }

    private void MaxFallSpeed()
    {
        if (rb.linearVelocityY < maxFallSpeed)
        {
            rb.linearVelocityY = maxFallSpeed;
        }
    }

    private void Slide()
    {
        if (!states.isCanMove || !states.slideIsUnlocked) { return; }

        if (!states.isJumped && !states.isGrounded && moveInput.x != 0 && states.isSlide == PlayerStates.Sides.none)
        {
            if (moveInput.x < 0 && IsTouchingWall() == PlayerStates.Sides.left)
            {
                states.isSlide = PlayerStates.Sides.left;
                states.isJumped = false;
            }
            else if (moveInput.x > 0 && IsTouchingWall() == PlayerStates.Sides.right)
            {
                states.isSlide = PlayerStates.Sides.right;
                states.isJumped = false;
            }
        }

        if (states.isSlide != PlayerStates.Sides.none)
        {
            if (IsTouchingWall() == PlayerStates.Sides.none || states.isGrounded)
            {
                states.isSlide = PlayerStates.Sides.none;
                bufferTimer = slideBuffer;
            } 

            else if ((moveInput.x > 0 && states.isSlide == PlayerStates.Sides.left)
                || (moveInput.x < 0 && states.isSlide == PlayerStates.Sides.right))
            {
                states.isSlide = PlayerStates.Sides.none;
                bufferTimer = slideBuffer;
            }
            else
            {
                rb.linearVelocityY = slideSpeed;
            }
        }
    }

    private PlayerStates.Sides IsTouchingWall()
    {
        if (leftBotCheck.IsTouchingLayers(groundLayer) || leftTopCheck.IsTouchingLayers(groundLayer))
        {
            return PlayerStates.Sides.left;
        }
        if (rightBotCheck.IsTouchingLayers(groundLayer) || rightTopCheck.IsTouchingLayers(groundLayer))
        {
            return PlayerStates.Sides.right;
        }
        return PlayerStates.Sides.none;
    }

    private void SlideBufferTimer()
    {
        if (bufferTimer > slideBuffer) return;
        if (bufferTimer > 0)
        {
            bufferTimer -= Time.fixedDeltaTime;
        }
        else
        {
            bufferTimer = slideBuffer + 1f;
        }
    }

    public bool CanWallJump()
    {
        if (bufferTimer > 0 && bufferTimer <= slideBuffer) return true;
        return false;
    }
}
