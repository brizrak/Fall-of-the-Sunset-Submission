using Player.Abilities;
using UnityEngine;

namespace Player.Scripts.States
{
    public class PlayerStates : MonoBehaviour
    {
        [HideInInspector] public GroundState ground;
        [HideInInspector] public MovementState movement;
        [HideInInspector] public ViewDirection direction;
        /*[HideInInspector]*/ public bool isCanMove;
        public Ability currentAbility;

        private void Awake()
        {
            // Изменить на проверку
            ground = GroundState.Grounded;
            movement = MovementState.Idle;
            direction = ViewDirection.Left;
            isCanMove = true;
            currentAbility = null;
        }

        public void ChangeAbility(Ability ability)
        {
            currentAbility?.Stop();
            currentAbility = ability;
        }

        public void StopAbility()
        {
            currentAbility = null;
        }
    }
}