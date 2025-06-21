using UnityEngine;

namespace Player
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;

        private bool isActive = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive) return;
            if (!other.CompareTag("Player")) return;
            isActive = true;
            playerController.SetCheckpoint(transform.position);
        }
    }
}