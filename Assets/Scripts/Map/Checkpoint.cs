using System;
using UnityEngine;
using Player;

namespace Map
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private AnimationClip _animation;
        
        private PlayerController _playerController;
        private Animator _animator;

        [HideInInspector] public bool isActive = false;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive) return;
            if (!other.CompareTag("Player")) return;
            _playerController = other.GetComponent<PlayerController>();
            if (!_playerController) return;
            _playerController.SetCheckpoint(this);
            isActive = true;
            _animator.Play(_animation.name);
        }
    }
}