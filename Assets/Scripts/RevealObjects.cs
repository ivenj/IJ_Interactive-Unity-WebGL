using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevealObjects : MonoBehaviour
{
    public bool autoReveal;

    public GameObject levelObjects_1;
    public GameObject levelObjects_2;

    public AudioSource[] revealSFX;
    public PortalScript portalScript;

    public float revealTime = 2f;
    public float objectOffset;

    bool objectsRevealed_1;
    bool objectsRevealed_2;
    bool triggerEnter;

    void Start()
    {

        if (autoReveal)
        {
            levelObjects_1.SetActive(false);
            Invoke("RevealObject", revealTime);
        }
    }

    void Update()
    {
        if (!autoReveal && InputManager.space && triggerEnter)
        {
            if (SceneManager.GetSceneByBuildIndex(1).isLoaded && !objectsRevealed_1)
            {
                Invoke("RevealObject", revealTime);
                objectsRevealed_1 = true;
            }
            if ((SceneManager.GetSceneByBuildIndex(2).isLoaded && !objectsRevealed_2)
                || (portalScript.scenesLoaded > 1 && !objectsRevealed_2))
            {
                Invoke("RevealObject", revealTime);
                objectsRevealed_2 = true;
            }
        }
    }

    void RevealObject()
    {
        foreach (AudioSource sfx in revealSFX)
        {
            sfx.Play();
        }

        Invoke("ObjectOffset", objectOffset);
    }

    void ObjectOffset()
    {
        if (portalScript.scenesLoaded == 1)
        {
            levelObjects_1.SetActive(true);
        }
        else if (portalScript.scenesLoaded > 1)
        {
            levelObjects_2.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        triggerEnter = false;
    }
}
