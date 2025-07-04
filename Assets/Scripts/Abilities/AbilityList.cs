﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Player.Movement;

namespace Abilities
{
    public class AbilityList : MonoBehaviour
    {
        [SerializeField] private int dashPriority;
        [SerializeField] private int jumpPriority;
        [SerializeField] private int slidePriority;
        [SerializeField] private int wallJumpPriority;

        private List<Ability> _abilities;
        public int GetMaxPriority => _abilities?.Any() == true ? _abilities.Max(a => a.priority) : 0;

        private void Awake()
        {
            _abilities = new List<Ability>();
            _abilities.AddRange(GetComponents<Ability>());

            var dash = _abilities.Find(a => a is Dash);
            dash.priority = dashPriority;

            var jump = _abilities.Find(a => a is Jump);
            jump.priority = jumpPriority;

            var slide = _abilities.Find(a => a is Slide);
            slide.priority = slidePriority;

            var wallJump = _abilities.Find(a => a is WallJump);
            wallJump.priority = wallJumpPriority;
        }
    }
}