using UnityEngine;
using Animations;
using States;

namespace Player
{
    public class PlayerController : Character.CharacterController
    {
        [SerializeField] private AnimationPreset animationPreset;
        // [SerializeField] private SafePositionRecorder safePositionRecorder; // later

        private Rigidbody2D _rb;
        private PlayerStates _states;
        private Inaction _inaction;
        private AnimationController _animationController;
        private Vector3 lastCheckpoint;
        public void SetCheckpoint(Vector3 checkpoint) => lastCheckpoint = checkpoint;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _states = GetComponent<PlayerStates>();
            _inaction = GetComponent<Inaction>();
            _animationController = GetComponent<AnimationController>();
        }

        protected override void HandleDeath()
        {
            _inaction.TryActivate();
            _states.isCanMove = false;
            characterCollider.enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
            Invoke(nameof(Respawn), 1f);
        }

        private void Respawn()
        {
            transform.position = lastCheckpoint;
            _animationController.HandleAnimation(animationPreset);
            health.ResetHealth();
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            characterCollider.enabled = true;
            _states.isCanMove = true;
            _states.StopAbility();
            
            // safePositionRecorder.SetStablePosition(transform.position); // later
        }
    }
}