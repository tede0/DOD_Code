using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotate : MonoBehaviour
{
    [SerializeField] private Transform car;
    
    private void Update()
    {
        Vector3 newPos = new Vector3(1, car.transform.position.y, car.transform.position.z);
        transform.position = newPos;
    }
}
