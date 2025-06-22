using Health;
using UnityEngine;
using HealthSystem;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] protected BaseHealth health;
        [SerializeField] protected Collider2D characterCollider;
        
        private HurtBox _hurtBox;

        protected virtual void Start()
        {
            health.onDeath.AddListener(HandleDeath);
            _hurtBox = characterCollider.GetComponent<HurtBox>();
            _hurtBox.onHit.AddListener(TakeDamage);
        }

        protected virtual void HandleDeath()
        {
            gameObject.SetActive(false);
        }
        
        private void TakeDamage(int damage) => health.TakeDamage(damage);
    }
}