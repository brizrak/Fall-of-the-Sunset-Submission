using UnityEngine;
using Player.States;

namespace Player.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public bool isUnlock;

        [HideInInspector] public int priority;
        [SerializeField] private float cooldown;
        protected float _lastUseTime;
        protected PlayerStates _states;

        protected virtual void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _lastUseTime = Time.time - cooldown;
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

        protected abstract void OnActivate();

        protected void Deactivate() => _states.DeactivateAbility(); 

        public abstract void Stop();

        protected virtual bool CustomCheck() => true;

        protected virtual void PreActivateAction() {}
    }
}