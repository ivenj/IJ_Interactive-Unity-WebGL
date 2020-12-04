using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI textTarget;
    public string objectName;
    public string replaceText;

    WwiseController wwiseController;
    string eventName;
    bool controllerDetected;
    bool sceneChanged;


    void Start()
    {
        wwiseController = GetComponent<WwiseController>();

        textTarget.gameObject.SetActive(false);    

        if (replaceText.Length != 0)
        {
            textTarget.text = replaceText;
        }
    }

    void Update()
    {
        UpdateText();
        DetectController();
    }

    private void OnTriggerEnter(Collider other)
    {
        textTarget.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        textTarget.gameObject.SetActive(false);
    }

    void UpdateText()
    {
        if (textTarget.text == "[Space]" || textTarget.text == "[X]")
        {
            if (InputManager.mouseAndKeyboardInput)
            {
                textTarget.text = "[Space]";
            }
            else if (InputManager.joystickInput)
            {
                textTarget.text = "[X]";
            }
        }


        if (textTarget.text == "[Space] to play" || textTarget.text == "[X] to play")
        {
            if (InputManager.mouseAndKeyboardInput)
            {
                textTarget.text = "[Space] to play";
            }
            else if (InputManager.joystickInput)
            {
                textTarget.text = "[X] to play";
            }
        }

        if (textTarget.text == "[T] to enter Movie Mode" || textTarget.text == "[Y] to enter Movie Mode")
        {
            if (InputManager.mouseAndKeyboardInput)
            {
                textTarget.text = "[T] to enter Movie Mode";
            }
            else if (InputManager.joystickInput)
            {
                textTarget.text = "[Y] to enter Movie Mode";
            }
        }

    }

    void DetectController()
    {
        if (!controllerDetected && (InputManager.mouseAndKeyboardInput || InputManager.joystickInput))
        {
            SetTabletText();
            controllerDetected = true;
        }

        if (!sceneChanged && SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            controllerDetected = false;
            sceneChanged = true;
        }

        if (sceneChanged && SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            controllerDetected = false;
            sceneChanged = false;
        }
    }

    void SetTabletText()
    {
        if (replaceText == "MM" || replaceText == "TV")
        {
            if (InputManager.mouseAndKeyboardInput)
            {
                textTarget.text = "[Space] to play '" + wwiseController.eventName + "'";
            }
            else if (InputManager.joystickInput)
            {
                textTarget.text = "[X] to play '" + wwiseController.eventName + "'";
            }
    
        }
    }

}
