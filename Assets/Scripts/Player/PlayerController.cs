using UnityEngine;
using Animations;
using Map;
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
        private Checkpoint lastCheckpoint;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _states = GetComponent<PlayerStates>();
            _inaction = GetComponent<Inaction>();
            _animationController = GetComponent<AnimationController>();
        }

        protected override void Start()
        {
            base.Start();
            FindForCheckpoint();
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
            MoveToCheckpoint();
            _animationController.HandleAnimation(animationPreset);
            health.ResetHealth();
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            characterCollider.enabled = true;
            _states.isCanMove = true;
            _states.StopAbility();
            
            // safePositionRecorder.SetStablePosition(transform.position); // later
        }

        public void SetCheckpoint(Checkpoint checkpoint)
        {
            if (lastCheckpoint) lastCheckpoint.isActive = false;
            lastCheckpoint = checkpoint;
        }

        private void FindForCheckpoint()
        {
            SetCheckpoint(FindAnyObjectByType<Checkpoint>());
            MoveToCheckpoint();
        }
        
        private void MoveToCheckpoint()
        {
            transform.position = lastCheckpoint.transform.position;
        }
    }
}