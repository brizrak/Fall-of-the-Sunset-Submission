using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    public class HurtBox : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> onHit;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out DamageDealer dealer)) onHit.Invoke(dealer.GetDamage());
        }
    }
}