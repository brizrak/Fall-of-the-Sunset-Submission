using UnityEngine;
using Player;

namespace Map
{
    public class Checkpoint : MonoBehaviour
    {
        private PlayerController _playerController;

        [HideInInspector] public bool isActive = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive) return;
            if (!other.CompareTag("Player")) return;
            _playerController = other.GetComponent<PlayerController>();
            if (!_playerController) return;
            _playerController.SetCheckpoint(this);
            isActive = true;
        }
    }
}