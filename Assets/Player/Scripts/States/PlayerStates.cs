using Player.Abilities;
using UnityEngine;

namespace Player.Scripts.States
{
    public class PlayerStates : MonoBehaviour
    {
        public Ground ground;
        [HideInInspector] public Movement movement;
        [HideInInspector] public Direction direction;
        /*[HideInInspector]*/ public bool isCanMove;
        public Ability currentAbility;

        private void Awake()
        {
            // Изменить на проверку
            ground = Ground.Grounded;
            movement = Movement.Idle;
            direction = Direction.Left;
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