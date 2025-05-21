using UnityEngine;
using Player.Movement;
using Player.Abilities;

namespace Player.States
{
    [RequireComponent(typeof(JumpManager), typeof(AbilityList))]
    public class PlayerStates : MonoBehaviour
    {
        /*[HideInInspector]*/ public Ground ground;
        /*[HideInInspector]*/ public Moving moving;
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
            moving = Moving.Idle;
            direction = Direction.Right;
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