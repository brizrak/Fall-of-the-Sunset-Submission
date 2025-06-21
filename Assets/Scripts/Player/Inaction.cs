using System.Collections;
using Abilities;

namespace Player
{
    public class Inaction : Ability
    {
        protected override void OnActivate()
        {
            base.OnActivate();
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            while (true)
            {
                yield return null;
            }
        }

        public override void Stop() => StopAllCoroutines();
    }
}