using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSize : MonoBehaviour
{
    public PortalScript portalScript;

    void Start()
    {

    }

    void Update()
    {
        if (portalScript.scenesLoaded > 1)
        {
            transform.localScale = new Vector3(50.0f, 0.5f, 50.0f);
        }
    }
}
