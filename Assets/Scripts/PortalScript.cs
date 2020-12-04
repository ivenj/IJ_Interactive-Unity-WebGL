using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalScript : MonoBehaviour
{
    public AudioSource wwiseEvent_SFX;

    WwiseController wwiseController;
    PortalTrigger portalTrigger;
    Light directionalLight;
    Coroutine sendPlayerDown;
    Coroutine movePortalSphereToCenterCoRoutine;

    float moveSpeed = 0.2f;
    int sceneIndex = 1;

    bool movePortalSphereToCenter;
    bool movePortalSphereBack;
    bool levelChanged;
    bool scenesChanged;


    void Start()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        portalTrigger = GameObject.Find("Portal Trigger").GetComponent<PortalTrigger>();
        wwiseController = GetComponent<WwiseController>();
    }

    void Update()
    {
        if (movePortalSphereToCenter && sceneIndex == 2 && SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            MovePortalSphereToCenter();
        }

        if (movePortalSphereBack && sceneIndex == 2)
        {
            MovePortalSphereBack();
        }

    }

    public void TriggerEnter()
    {
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded && !levelChanged)
        {
            Invoke("ChangeToScene2", 8.2f);
            levelChanged = true;
        }

        if (SceneManager.GetSceneByBuildIndex(2).isLoaded && !levelChanged)
        {
            Invoke("ChangeToScene1", 8.2f);
            levelChanged = true;
        }
    }

    public void TriggerExit()
    {
        movePortalSphereToCenterCoRoutine = StartCoroutine(MovePortalSphereToCenterCoRoutine());
    }

    void ChangeToScene2()
    {
        sceneIndex = 2;
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(1);
        directionalLight.intensity = 0f;
        wwiseEvent_SFX.Play();
        sendPlayerDown = StartCoroutine(SendPlayerDown());
        scenesChanged = true;
    }

    void ChangeToScene1()
    {
        sceneIndex = 1;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(2);
        directionalLight.intensity = 0.5f;
        wwiseEvent_SFX.Play();
        sendPlayerDown = StartCoroutine(SendPlayerDown());
        scenesChanged = true;
    }

    IEnumerator SendPlayerDown()
    {
        yield return new WaitForSeconds(6.5f);
        portalTrigger.isFlyingDown = true;
        levelChanged = false;
        if (sceneIndex == 1)
        {
            scenesChanged = false;
        }
        if (sceneIndex == 2)
        {
            scenesChanged = false;
        }
    }

    IEnumerator MovePortalSphereToCenterCoRoutine()
    {
        yield return new WaitForSeconds(1.75f);
        movePortalSphereToCenter = true;
        movePortalSphereBack = false;
    }

    void MovePortalSphereToCenter()
    {
        transform.Translate(new Vector3(0, 5, 11) * Time.deltaTime * moveSpeed, Space.World);

        if (transform.position.y >= 11)
        {
            transform.position = new Vector3(transform.position.x, 11, transform.position.z);
        }
        if (transform.position.z >= 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        if (transform.position.y >= 11 && transform.position.z >= 0)
        {
            StopCoroutine(movePortalSphereToCenterCoRoutine);
            portalTrigger.portalMovedToCenter = true;
        }
    }

    public IEnumerator MovePortalSphereBackCoRoutine()
    {
        yield return new WaitForSeconds(1f);
        movePortalSphereBack = true;
        movePortalSphereToCenter = false;
    }

    void MovePortalSphereBack()
    {
        transform.Translate(new Vector3(0, -5, -11) * Time.deltaTime * moveSpeed, Space.World);

        if (transform.position.y <= 6)
        {
            transform.position = new Vector3(transform.position.x, 6, transform.position.z);
        }
        if (transform.position.z <= -11)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -11);
        }

        portalTrigger.portalMovedBack = true;
    }
}

