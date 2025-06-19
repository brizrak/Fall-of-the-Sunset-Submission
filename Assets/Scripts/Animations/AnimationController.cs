using States;
using UnityEngine;

namespace Animations
{
    public class AnimationController : BaseAnimationController
    {
        [SerializeField] private int statesLayer;
        [SerializeField] private int abilitiesLayer;
        
        [Header("Animations")]
        [SerializeField] private AnimationPreset idleAnimation;
        [SerializeField] private AnimationPreset runAnimation;
        [SerializeField] private AnimationPreset fallAnimation;
        [SerializeField] private AnimationPreset slideAnimation;
        
        private PlayerStates _states;
        private Direction _currentDirection;
        private SpriteRenderer _currentRenderer;
        
        private bool _currentAnimationIsMirrored;
        private string _currentAnimationName;

        protected override void Awake()
        {
            base.Awake();
            _states = GetComponent<PlayerStates>();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void HandleAnimation(string animationName, float blendTime, int layer = 0)
        {
            if (_currentAnimationName != animationName)
            {
                _currentRenderer = GetComponentInChildren<SpriteRenderer>();
                _currentAnimationName =  animationName;
            }
            MirrorAnimation();
            
            base.HandleAnimation(animationName, blendTime, layer);
        }

        public override void HandleAnimation(AnimationPreset preset)
        {
            _currentAnimationIsMirrored = preset.isMirrored;
            
            base.HandleAnimation(preset);
        }

        protected override void HandleStateAnimation(AnimationPreset preset)
        {
            if (preset.AnimationName == _currentAnimation && _currentDirection == _states.direction) return;
            _currentAnimation = preset.AnimationName;
            _currentDirection = _states.direction;
            HandleAnimation(preset.AnimationName, preset.blendTime, 0);
        }

        protected override void CheckForDefault()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0) is { loop: false, normalizedTime: < 1 }) return;

            switch (_states.ground)
            {
                case Ground.Falling:
                    HandleStateAnimation(fallAnimation);
                    break;
                case Ground.Sliding:
                    HandleAnimation(slideAnimation);
                    break;
                default:
                {
                    HandleStateAnimation(_states.moving is Moving.Idle ? idleAnimation : runAnimation);
                    break;
                }
            }
        }

        private void MirrorAnimation()
        {
            _currentRenderer.flipX = _states.direction is Direction.Left;
        }
        
        public void MirrorAnimationDuringRun()
        {
            if (!_currentAnimationIsMirrored) return;
            if (_states.direction == _currentDirection) return;
            MirrorAnimation();
        }
    }
}