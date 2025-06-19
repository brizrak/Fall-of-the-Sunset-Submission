using UnityEngine;
using States;

namespace Player.Movement
{
    [RequireComponent(typeof(Slide), typeof(PlayerStates))]
    public class SlideManager : MonoBehaviour
    {
        private PlayerStates _states;
        private Slide _slide;
        private CheckForTouch _checkForTouch;
        private Direction? _direction;

        private void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _slide = GetComponent<Slide>();
            _checkForTouch = GetComponent<CheckForTouch>();
        }

        private void FixedUpdate()
        {
            CheckForSlide();
        }

        private void CheckForSlide()
        {
            if (!_states.isCanMove) return;
            if ((_states.ground is not Ground.Falling) || (_states.moving is not Moving.Walk)) return;

            _direction = _checkForTouch.IsTouchingWall();
            // if (_direction is null) return; // optimization?

            if (_states.direction == Direction.Left && _direction is Direction.Left ||
                _states.direction == Direction.Right && _direction is Direction.Right)
            {
                _slide.TryActivate();
            }
        }
    }
}