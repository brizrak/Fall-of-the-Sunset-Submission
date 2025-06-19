using UnityEngine;
using HealthSystem;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] protected Health health;
        [SerializeField] protected Collider2D characterCollider;

        private void Start()
        {
            health.onDeath.AddListener(HandleDeath);
        }

        protected virtual void HandleDeath()
        {
            gameObject.SetActive(false);
        }
    }
}