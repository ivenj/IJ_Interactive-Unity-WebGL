using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static bool mouseAndKeyboardInput;
    public static bool joystickInput;

    public static bool space;
    public static bool shift;
    public static bool movieMode;
    public static bool callPortal;
    public static bool firstPerson;
    public static bool escape;

    bool sceneChanged;

    void Update()
    {
        MouseAndKeyboardInput();
        JoystickInput();

        PressSpace();
        PressShift();
        PressMovieMode();
        PressCallPortal();
        PressFirstPerson();
        PressFullScreen();
        PressEscape();
    }

    void MouseAndKeyboardInput()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            mouseAndKeyboardInput = true;
            joystickInput = false;
        }


        if (!sceneChanged && SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            mouseAndKeyboardInput = false;
            joystickInput = false;
            sceneChanged = true;
        }
        if (sceneChanged && SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            mouseAndKeyboardInput = false;
            joystickInput = false;
            sceneChanged = false;
        }
    }

    void JoystickInput()
    {
        if (Input.GetAxis("LS X") != 0 || Input.GetAxis("LS Y") != 0 || Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            joystickInput = true;
            mouseAndKeyboardInput = false;
        }
    }

    void PressSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton2))
            space = true;
        else
            space = false;
    }

    void PressShift()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.JoystickButton0))
            shift = true;
        else
            shift = false;
    }

    void PressMovieMode()
    {
        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.JoystickButton3))
            movieMode = true;
        else
            movieMode = false;
    }

    void PressCallPortal()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton1))
            callPortal = true;
        else
            callPortal = false;
    }

    void PressFirstPerson()
    {
        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.JoystickButton9))
            firstPerson = true;
        else
            firstPerson = false;
    }

    void PressFullScreen()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton9))
            Screen.fullScreen = !Screen.fullScreen;
    }

    void PressEscape()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton7))
            escape = true;
        else
            escape = false;
    }
}
