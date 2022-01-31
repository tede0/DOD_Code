using UnityEngine;
using System.Collections;
 
public class LaserPointer : MonoBehaviour {
    
    private LineRenderer lineRenderer;

    public GameObject laserDot;
    
    private void Start() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        laserDot.SetActive(false);
    }
    
    private void Update() 
    {
        HandleLaser();
    }

    private void HandleLaser()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lineRenderer.enabled = true;
            laserDot.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.enabled = false;
            laserDot.SetActive(false);
        }
        
        
        
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            lineRenderer.SetPosition(1, hit.point);
            laserDot.transform.position = hit.point;
        }
        else
        {
            lineRenderer.SetPosition(1, transform.forward * 5000);
        }
    }
}