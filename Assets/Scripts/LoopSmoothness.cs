using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSmoothness : MonoBehaviour
{
    Material blackTablet;
    float smoothness = 0f;
    bool smoothUp;

    void Start()
    {
        blackTablet = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (smoothness <= 0)
        {
            smoothUp = true;
        }
        if (smoothness >= 1)
        {
            smoothUp = false;
        }
        
        if (smoothUp)
        {
            smoothness += 0.003f;
        }
        if (!smoothUp)
        {
            smoothness -= 0.003f;
        }

        blackTablet.SetFloat("_Glossiness", smoothness);
        blackTablet.SetFloat("_Metallic", smoothness);

    }
}
