using System;
using UnityEngine;
using Player.States;
using UnityEngine.Events;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerStates))]
    public class Move : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float maxFallSpeed;

        private Rigidbody2D _rb;
        private PlayerStates _states;
        private float _currentSpeed;
        private float _currentAcceleration;
        private Vector2 _moveInput;
        public void MoveInput(Vector2 moveInput) => _moveInput = moveInput;
        public UnityEvent onDirectionChange;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _states = GetComponent<PlayerStates>();
        }

        private void FixedUpdate()
        {
            PlayerMove();
            MaxFallSpeed();
            MoveSide();
        }

        private void PlayerMove()
        {
            if (!_states.isCanMove)
            {
                _currentSpeed = 0;
                _states.moving = Moving.Idle;
                return;
            }

            _states.moving = _moveInput.x == 0 ? Moving.Idle : Moving.Walk;
            _currentAcceleration = _moveInput.x != 0 ? acceleration : deceleration;

            _currentSpeed = Mathf.MoveTowards(_currentSpeed, Math.Sign(_moveInput.x),
                _currentAcceleration * Time.fixedDeltaTime);

            _rb.linearVelocityX = _currentSpeed * moveSpeed;
        }

        private void MoveSide()
        {
            if (!_states.isCanMove) return;
            onDirectionChange?.Invoke();

            _states.direction = _moveInput.x switch
            {
                < 0 => Direction.Left,
                > 0 => Direction.Right,
                _ => _states.direction
            };
        }

        private void MaxFallSpeed()
        {
            if (_rb.linearVelocityY < maxFallSpeed)
            {
                _rb.linearVelocityY = maxFallSpeed;
            }
        }
    }
}