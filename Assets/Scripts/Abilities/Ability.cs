using UnityEngine;
using States;
using Animations;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public bool isUnlock;

        [SerializeField] private float cooldown;
        [SerializeField] private AnimationPreset animationPreset;
        
        [HideInInspector] public int priority;
        
        protected float _lastUseTime;
        protected PlayerStates _states;
        protected AnimationController _animationController;

        protected virtual void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _lastUseTime = Time.time - cooldown;
            _animationController = GetComponent<AnimationController>();
        }

        protected bool CanActivate()
        {
            return isUnlock && Time.time > _lastUseTime + cooldown && _states.GetPriority() > priority && CustomCheck();
        }

        public virtual void TryActivate()
        {
            if (!CanActivate()) return;
            PreActivateAction();
            _lastUseTime = Time.time;
            _states.ChangeAbility(this);
            OnActivate();
        }

        protected virtual void OnActivate() => _animationController.HandleAnimation(animationPreset);

        protected void Deactivate() => _states.DeactivateAbility();

        public abstract void Stop();

        protected virtual bool CustomCheck() => true;

        protected virtual void PreActivateAction() {}
    }
}