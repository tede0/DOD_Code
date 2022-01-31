using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasMaskSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gasMask;
    [SerializeField] private GameObject[] spots;

    void Start()
    {
        Instantiate(gasMask, RandomSpot(), Quaternion.identity);
    }

    private Vector3 RandomSpot()
    {
        return spots[Random.Range(0, spots.Length)].transform.position;
    }

}
