using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawns : MonoBehaviour
{
    [Serializable]
    public class SpawnInfo
    {
        public GameObject prefab;
        public Transform spawnPoint;
    }
    
    [SerializeField] private List<SpawnInfo> spawns;

    public void Spawn()
    {
        foreach (var couple in spawns.Where(couple => couple.prefab != null && couple.spawnPoint != null))
        {
            Instantiate(couple.prefab, couple.spawnPoint.position, couple.spawnPoint.rotation);
        }
    }

    private void Start()
    {
        Spawn();
    }
}
