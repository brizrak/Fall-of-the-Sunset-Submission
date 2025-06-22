using UnityEngine;

namespace Health
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private int damage;
        // [SerializeField] private DamageType type;

        public int GetDamage() => damage;
    }
}