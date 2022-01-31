using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = System.Random;

public class ObstacleSpawnSingle : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacleList;
    private void Start()
    {
        GameObject obstacleToSpawn = PickRandomObstacle();
        Instantiate(obstacleToSpawn, gameObject.transform.position, Quaternion.identity, transform);
    }
    
    public GameObject PickRandomObstacle()
    {
        Random random = new Random();
        int randItemToPick = random.Next(0, obstacleList.Length);
        return obstacleList[randItemToPick];
    }
}
