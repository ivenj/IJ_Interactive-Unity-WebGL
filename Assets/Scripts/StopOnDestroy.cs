using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnDestroy : MonoBehaviour
{
    //AkAmbient akAmbient;

    void Start()
    {
        //akAmbient = GetComponent<AkAmbient>();
    }

    void OnDestroy()
    {
        //akAmbient.Stop(0);
    }
}
