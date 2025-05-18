using UnityEngine;

namespace Player.Abilities
{
    public abstract class JumpAbility : Ability
    {
        public override void TryActivate()
        {
            if (!CanActivate()) return;
            _lastUseTime = Time.time;
            _states.StopAbility();
            IsJumping(true);
            OnActivate();
        }

        protected abstract void IsJumping(bool jumping);
    }
}