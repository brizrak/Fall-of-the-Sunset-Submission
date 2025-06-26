using System;
using UnityEngine;

namespace Player
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;

        public Action<GameObject> OnPlayerSpawned;

        private void Start()
        {
            var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            OnPlayerSpawned?.Invoke(player);
        }
    }
}