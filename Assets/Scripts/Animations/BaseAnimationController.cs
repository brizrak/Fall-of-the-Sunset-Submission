using UnityEngine;

namespace Animations
{
    public abstract class BaseAnimationController : MonoBehaviour
    {
        protected Animator _animator;
        protected string _currentAnimation;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        protected virtual void HandleAnimation(string animationName, float blendTime, int layer = 0)
        {
            _animator.CrossFade(animationName, blendTime, layer, 0f); // CrossFadeInFixedTime ??
        }

        public void HandleAnimation(AnimationPreset preset)
        {
            HandleAnimation(preset.AnimationName, preset.blendTime, 0);
        }

        protected virtual void HandleLoopAnimation(AnimationPreset preset)
        {
            if (preset.AnimationName == _currentAnimation) return;
            _currentAnimation = preset.AnimationName;
            HandleAnimation(preset.AnimationName, preset.blendTime, 0);
        }

        protected abstract void CheckForDefault();

        protected void Update() => CheckForDefault();
    }
}