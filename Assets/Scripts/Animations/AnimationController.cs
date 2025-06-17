using Player.States;
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

        protected override void Awake()
        {
            base.Awake();
            _states = GetComponent<PlayerStates>();
        }

        protected override void HandleAnimation(string animationName, float blendTime, int layer = 0)
        {
            animationName += _states.direction is Direction.Left ? "_L" : "_R";
            
            base.HandleAnimation(animationName, blendTime, layer);
        }

        protected override void HandleLoopAnimation(AnimationPreset preset)
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
                    HandleLoopAnimation(fallAnimation);
                    break;
                case Ground.Sliding:
                    HandleAnimation(slideAnimation);
                    break;
                default:
                {
                    HandleLoopAnimation(_states.moving is Moving.Idle ? idleAnimation : runAnimation);
                    break;
                }
            }
        }
    }
}