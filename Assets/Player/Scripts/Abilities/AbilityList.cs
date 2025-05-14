using System.Collections.Generic;
using UnityEngine;

namespace Player.Abilities
{
    public class AbilityList : MonoBehaviour
    {
        [SerializeField] private int dashPriority;

        private List<Ability> _abilities;

        private void Awake()
        {
            _abilities = new List<Ability>();
            _abilities.AddRange(GetComponents<Ability>());

            var dash = _abilities.Find(a => a is Dash);
            dash.priority = dashPriority;
        }
    }
}