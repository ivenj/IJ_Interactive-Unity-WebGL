using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwisePostEvent : MonoBehaviour
{
    public AudioSource wwiseEvent_1;
    public AudioSource wwiseEvent_2;
    public AudioSource wwiseEvent_3;

    bool eventPlayable_1;

    private void Start()
    {
        eventPlayable_1 = true;
    }

    public void PostEvent_1()
    {
        if (eventPlayable_1)
        {
            wwiseEvent_1.Play();
            eventPlayable_1 = false;
            Invoke("EventPlayability_1", 0.25f);
        }
    }

    void EventPlayability_1()
    {
        eventPlayable_1 = true;
        wwiseEvent_1.pitch = Random.Range(0.8f, 1.3f);
    }

    public void PostEvent_2()
    {
        wwiseEvent_2.Play();
    }

    public void PostEvent_3()
    {
        wwiseEvent_3.Play();
    }

}
