using Player.Scripts.States;
using UnityEngine;

namespace Player.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] private AbilityData data;
        public bool isUnlock; // change

        [HideInInspector] public int priority;
        private float _cooldown;
        private float _lastUseTime;
        protected PlayerStates _states;

        protected virtual void Awake()
        {
            _states = GetComponent<PlayerStates>();
            _cooldown = data.cooldown;
            priority = data.priority;
            _lastUseTime = Time.time - _cooldown;
        }

        private bool CanActivate()
        {
            Debug.Log(isUnlock && Time.time > _lastUseTime + _cooldown 
                               && (_states.currentAbility == null || _states.currentAbility.priority <= priority));
            return isUnlock && Time.time > _lastUseTime + _cooldown 
                            && (_states.currentAbility == null || _states.currentAbility.priority <= priority); // == or is
        }

        public void TryActivate()
        {
            if (!CanActivate()) return;
            _lastUseTime = Time.time;
            _states.ChangeAbility(this);
            OnActivate();
        }

        protected abstract void OnActivate();

        public void Deactivate()
        {
            _states.StopAbility();
        }

        public abstract void Stop();
    }
}