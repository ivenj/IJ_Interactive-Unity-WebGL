using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTabletTrigger : MonoBehaviour
{
    PortalTrigger portalTrigger;

    void Start()
    {
        portalTrigger = GameObject.Find("Portal Trigger").GetComponent<PortalTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        portalTrigger.TriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        portalTrigger.TriggerExit();
    }
}
