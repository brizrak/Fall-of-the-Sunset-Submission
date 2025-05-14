using System;
using Player.Scripts.States;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStates))]
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
    private PlayerStates _states;
    private float currentSpeed = 0;
    private float currentAcceleration;
    private float bufferTimer;
    private Vector2 moveInput;
    public void MoveInput(Vector2 moveInput) { this.moveInput = moveInput; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _states = GetComponent<PlayerStates>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        MaxFallSpeed();
        // Slide();
        MoveSide();
        SlideBufferTimer();
    }

    private void PlayerMove()
    {
        if (!_states.isCanMove) {
            currentSpeed = 0;
            return;
        }

        currentAcceleration = moveInput.x != 0 ? acceleration : decereration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, Math.Sign(moveInput.x), currentAcceleration * Time.fixedDeltaTime);

        rb.linearVelocityX = currentSpeed * moveSpeed;
    }

    public void MoveSide()
    {
        if (!_states.isCanMove) return;

        if (moveInput.x < 0)
        {
            _states.direction = Direction.Left;
        }
        else if (moveInput.x > 0)
        {
            _states.direction = Direction.Right;
        }
    }

    private void MaxFallSpeed()
    {
        if (rb.linearVelocityY < maxFallSpeed)
        {
            rb.linearVelocityY = maxFallSpeed;
        }
    }

    // private void Slide()
    // {
    //     if (!_states.isCanMove || !_states.slideIsUnlocked) { return; }
    //
    //     if (!_states.isJumped && !_states.isGrounded && moveInput.x != 0 && _states.isSlide == PlayerStatesOld.Sides.none)
    //     {
    //         if (moveInput.x < 0 && IsTouchingWall() == PlayerStatesOld.Sides.left)
    //         {
    //             _states.isSlide = PlayerStatesOld.Sides.left;
    //             _states.isJumped = false;
    //         }
    //         else if (moveInput.x > 0 && IsTouchingWall() == PlayerStatesOld.Sides.right)
    //         {
    //             _states.isSlide = PlayerStatesOld.Sides.right;
    //             _states.isJumped = false;
    //         }
    //     }
    //
    //     if (_states.isSlide != PlayerStatesOld.Sides.none)
    //     {
    //         if (IsTouchingWall() == PlayerStatesOld.Sides.none || _states.isGrounded)
    //         {
    //             _states.isSlide = PlayerStatesOld.Sides.none;
    //             bufferTimer = slideBuffer;
    //         } 
    //
    //         else if ((moveInput.x > 0 && _states.isSlide == PlayerStatesOld.Sides.left)
    //             || (moveInput.x < 0 && _states.isSlide == PlayerStatesOld.Sides.right))
    //         {
    //             _states.isSlide = PlayerStatesOld.Sides.none;
    //             bufferTimer = slideBuffer;
    //         }
    //         else
    //         {
    //             rb.linearVelocityY = slideSpeed;
    //         }
    //     }
    // }

    private PlayerStatesOld.Sides IsTouchingWall()
    {
        if (leftBotCheck.IsTouchingLayers(groundLayer) || leftTopCheck.IsTouchingLayers(groundLayer))
        {
            return PlayerStatesOld.Sides.left;
        }
        if (rightBotCheck.IsTouchingLayers(groundLayer) || rightTopCheck.IsTouchingLayers(groundLayer))
        {
            return PlayerStatesOld.Sides.right;
        }
        return PlayerStatesOld.Sides.none;
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
