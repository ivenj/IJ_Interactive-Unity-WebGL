using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTriggerDetection : MonoBehaviour
{
    PortalScript portalScript;

    void Start()
    {
        portalScript = GameObject.Find("Portal Sphere").GetComponent<PortalScript>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        portalScript.TriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        portalScript.TriggerExit();
    }
}
