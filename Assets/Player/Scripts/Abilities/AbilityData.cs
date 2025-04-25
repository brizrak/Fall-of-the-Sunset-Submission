using UnityEngine;

namespace Player.Abilities
{
    [CreateAssetMenu(fileName = "abilityData", menuName = "Ability/Data", order = 0)]
    public class AbilityData : ScriptableObject
    {
        public int priority;
        public float cooldown;
    }
}