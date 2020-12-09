using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool levelManagerIsActive;
    public bool levelDebug;
    public GameObject canvasTexts;

    public PortalScript portalScript;
    Scene scene1;
    Scene scene2;

    void Start()
    {
        Cursor.visible = false;
        canvasTexts.SetActive(true);
        //portalScript = GameObject.Find("Portal Sphere").GetComponent<PortalScript>();

        scene1 = SceneManager.GetSceneByBuildIndex(1);
        scene2 = SceneManager.GetSceneByBuildIndex(2);

        InitialSceneLoad();
    }

    void Update()
    {
        scene1 = SceneManager.GetSceneByBuildIndex(1);
        scene2 = SceneManager.GetSceneByBuildIndex(2);
        ManualSceneLoad();
    }

    void InitialSceneLoad()
    {
        if (scene2.isLoaded && levelManagerIsActive)
        {
            SceneManager.UnloadSceneAsync(2);
        }
        if (!scene1.isLoaded && levelManagerIsActive)
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }

    void ManualSceneLoad()
    {
        if (Input.GetKeyDown(KeyCode.L) && scene2.isLoaded && !scene1.isLoaded && levelDebug)
        {
            SceneManager.UnloadSceneAsync(2);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            portalScript.scenesLoaded++;
        }
        else if (Input.GetKeyDown(KeyCode.L) && scene1.isLoaded && !scene2.isLoaded && levelDebug)
        {
            SceneManager.UnloadSceneAsync(1);
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            portalScript.scenesLoaded++;
        }
    }
}
