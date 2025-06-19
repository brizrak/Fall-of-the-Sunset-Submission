using UnityEngine;
using Character;

namespace Player
{
    public class PlayerController : Character.CharacterController
    {
        // [SerializeField] private SafePositionRecorder safePositionRecorder; // later

        protected override void HandleDeath()
        {
            characterCollider.enabled = false;
            Respawn();
        }
        
        private void Respawn()
        {
            health.ResetHealth();
            characterCollider.enabled = true;
            // transform.position = LastCheckpoint;
            
            // safePositionRecorder.SetStablePosition(transform.position); // later
        }
    }
}