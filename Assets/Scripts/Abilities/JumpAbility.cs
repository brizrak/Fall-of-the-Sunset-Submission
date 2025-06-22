using UnityEngine;

namespace Abilities
{
    public abstract class JumpAbility : Ability
    {
        public override void TryActivate()
        {
            if (!CanActivate()) return;
            _states.StopAbility();
            _lastUseTime = Time.time;
            IsJumping(true);
            OnActivate();
        }

        protected abstract void IsJumping(bool jumping);
    }
}