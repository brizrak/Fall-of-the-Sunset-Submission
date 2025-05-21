using Player.Abilities;
using UnityEngine;

namespace Player.Scripts.States
{
    public class PlayerStates : MonoBehaviour
    {
        /*[HideInInspector]*/ public Ground ground;
        /*[HideInInspector]*/ public Movement movement;
        /*[HideInInspector]*/ public Direction direction;
        /*[HideInInspector]*/ public bool isCanMove;
        /*[HideInInspector]*/ public Ability currentAbility;
        
        private JumpManager _jumpManager;
        private AbilityList _abilityList;
        private int _priorityLimit;

        private void Awake()
        {
            // Изменить на проверку
            ground = Ground.Grounded;
            movement = Movement.Idle;
            direction = Direction.Left;
            isCanMove = true;
            currentAbility = null;
            _jumpManager = GetComponent<JumpManager>();
            _abilityList = GetComponent<AbilityList>();
        }

        private void Start()
        {
            _priorityLimit = _abilityList.GetMaxPriority + 1;
        }

        public int GetPriority()
        {
            return currentAbility?.priority ?? _jumpManager.GetPriority() ?? _priorityLimit;
        }

        public void ChangeAbility(Ability ability)
        {
            _jumpManager.StopJump();
            currentAbility?.Stop();
            currentAbility = ability;
        }

        public void DeactivateAbility()
        {
            currentAbility = null;
        }

        public void StopAbility()
        {
            currentAbility?.Stop(); 
            currentAbility = null;
        }
    }
}