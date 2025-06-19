using UnityEngine;
using UnityEngine.Events;

// ReSharper disable once CheckNamespace
namespace HealthSystem
{
    public class BaseHealth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int maxHealth;
        [SerializeField] private bool invincibleAfterDamage = false;
        [SerializeField] private float invincibilityDuration = 1f;

        [Header("Events")]
        public UnityEvent<int, int> onHealthChanged;
        public UnityEvent onTakeDamage;
        public UnityEvent onDeath;
        public UnityEvent onHeal;

        private int _currentHealth;
        private bool _isInvincible;
        private float _invincibilityTimer;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => maxHealth;

        private void Start()
        {
            ResetHealth();
        }

        private void Update()
        {
            if (!_isInvincible) return;
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0) _isInvincible = false;
        }

        public void TakeDamage(int health)
        {
            if (_isInvincible || _currentHealth <= 0) return;

            _currentHealth -= health;
            onHealthChanged?.Invoke(_currentHealth,  maxHealth);
            onTakeDamage?.Invoke();

            if (_currentHealth <= 0)
            {
                Die();
                return;
            }

            if (invincibleAfterDamage) StartInvincibility();
        }

        public void Heal(int health)
        {
            _currentHealth = Mathf.Min(maxHealth, _currentHealth + health);
            onHealthChanged?.Invoke(_currentHealth,  maxHealth);
            onHeal?.Invoke();
        }

        public void ChangeMaxHealth(int health)
        {
            maxHealth += health;
            _currentHealth += health;
            onHealthChanged?.Invoke(_currentHealth,  maxHealth);
        }

        private void StartInvincibility()
        {
            _isInvincible = true;
            _invincibilityTimer = invincibilityDuration;
        }

        private void Die() => onDeath?.Invoke();

        public void ResetHealth()
        {
            _currentHealth = maxHealth;
            _isInvincible = false;
            onHealthChanged?.Invoke(_currentHealth,  maxHealth);
        }
    }
}