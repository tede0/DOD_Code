using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacleList;
    private GameObject[] _spawnPoints;
    
    private void Awake()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("ObstacleSpawnPoint");
        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        foreach (var go in _spawnPoints)
        {
            Instantiate(PickRandomObstacle(), go.transform.position, Quaternion.identity);
        }
    }
    
    public GameObject PickRandomObstacle()
    {
        Random random = new Random();
        int randItemToPick = random.Next(0, obstacleList.Length);
        return obstacleList[randItemToPick];
    }
    
}
