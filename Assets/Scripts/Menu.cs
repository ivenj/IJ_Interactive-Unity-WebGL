using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

using TMPro;


public class Menu : MonoBehaviour
{
    public GameObject menuCanvas;
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI infoText;
    public GameObject defaultButton;

    //public Image blackScreen;
    public AudioSource[] mouseEnter;
    public AudioSource mouseClick;
    public AudioSource quitGame;
    public AudioSource toggleMenu;
    //public AudioSource stopAllEvents;

    Scene scene1;
    Scene scene2;
    GameObject player;
    MusicManager musicManager;
    VideoPlayer fadeToBlackVideo;
    bool menuToggled;
    bool quittingGame;
    bool mouseActivated;
    bool controllerActivated;
    bool defaultButtonSelected;
    bool selectedButtonDeselected;
    int mouseEnterIndex = 0;

    void Start()
    {
        menuCanvas.SetActive(false);
        mouseSensitivitySlider.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);

        scene1 = SceneManager.GetSceneByBuildIndex(1);
        scene2 = SceneManager.GetSceneByBuildIndex(2);
        player = GameObject.Find("Player");
        fadeToBlackVideo = GameObject.Find("Fade To Black Player").GetComponent<VideoPlayer>();
        fadeToBlackVideo.Pause();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    void Update()
    {
        ToggleMenu();
        QuitGameZoom();
        ShowCursor();
        UseMouse();
        UseController();
        ControlsText();

        if (controllerActivated && !defaultButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
            defaultButtonSelected = true;
            selectedButtonDeselected = false;
        }

        if (mouseActivated && !selectedButtonDeselected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            selectedButtonDeselected = true;
            defaultButtonSelected = false;
        }

    }

    public bool isWalkingForward;
    public bool isWalkingBackward;
    public bool isRunningForward;
    public bool isRunningBackward;
    public bool isTurningLeft;
    public bool isTurningRight;

    void ToggleMenu()
    {
        if (InputManager.escape && !menuToggled)
        {
            //Disable Animations & Movement
            player.GetComponent<PlayerMovement>().isWalkingForward = false;
            player.GetComponent<PlayerMovement>().isWalkingBackward = false;
            player.GetComponent<PlayerMovement>().isRunningForward = false;
            player.GetComponent<PlayerMovement>().isRunningBackward = false;
            player.GetComponent<PlayerMovement>().isTurningLeft = false;
            player.GetComponent<PlayerMovement>().isTurningRight = false;

            player.GetComponent<PlayerMovement>().moveAble = false;
            Invoke("EnablePlayerMovement", 0.2f);

            //Toggle Menu
            menuCanvas.SetActive(true);
            toggleMenu.Play();
            menuToggled = true;
        }

        else if (InputManager.escape && menuToggled || Input.GetKeyDown(KeyCode.JoystickButton1) && menuToggled)
        {
            menuCanvas.SetActive(false);
            toggleMenu.Play();
            menuToggled = false;
            Cursor.visible = false;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerMovement>().moveAble = true;

            //Reset 
            selectedButtonDeselected = false;
            defaultButtonSelected = false;

            //Deactivate toggled submenus
            mouseSensitivitySlider.gameObject.SetActive(false);
            controlsText.gameObject.SetActive(false);
            infoText.gameObject.SetActive(false);
            creditsText.gameObject.SetActive(false);
        }
    }

    void EnablePlayerMovement()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    void ShowCursor()
    {
        if (menuToggled && InputManager.mouseAndKeyboardInput && !InputManager.joystickInput)
        {
            Cursor.visible = true;
        }
        else if (menuToggled && InputManager.joystickInput && !InputManager.mouseAndKeyboardInput)
        {
            Cursor.visible = false;
        }
    }

    public void RestartGame()
    {
        //stopAllEvents.Post(this.gameObject);

        Invoke("ReloadScenes", .15f);
    }

    void ReloadScenes()
    {
        if (scene2.isLoaded)
        {
            SceneManager.UnloadSceneAsync(2);
        }
        else if (scene1.isLoaded)
        {
            SceneManager.UnloadSceneAsync(1);
        }

        SceneManager.LoadSceneAsync(0);
    }

    public void ShowControls()
    {
        //Disable other UI sections
        if (mouseSensitivitySlider.gameObject.activeSelf)
        {
            mouseSensitivitySlider.gameObject.SetActive(false);
        }
        if (creditsText.gameObject.activeSelf)
        {
            creditsText.gameObject.SetActive(false);
        }
        if (infoText.gameObject.activeSelf)
        {
            infoText.gameObject.SetActive(false);
        }

        //Toggle Controls Text
        if (!controlsText.gameObject.activeSelf)
        {
            controlsText.gameObject.SetActive(true);
        }
        else if (controlsText.gameObject.activeSelf)
        {
            controlsText.gameObject.SetActive(false);
        }
    }

    public void ControlsText()
    {
        if (controllerActivated)
        {
            controlsText.text = "XBOX CONTROLLER: " +
                                "\nMove Forward/Backward - LS Y" +
                                "\nTurn Left/ Right - LS/RS X" +
                                "\nLook Up/ Down - RS Y" +
                                "\nSprint - A" +
                                "\nZoom In / Out - Press LS -> RS Y" +
                                "\nToggle First Person - Press RS" +
                                "\nInteract - X" +
                                "\nMovie Mode - Y" +
                                "\nCall Portal - B";
        }
        else if (mouseActivated)
        {
            controlsText.text = "MOUSE AND KEYBOARD: " +
                                "\nToggle FullScreen - F" +
                                "\nMove Forward/Backward - WS" +
                                "\nTurn Left/ Right - AD + MouseX" +
                                "\nLook Up/ Down - Mouse Y" +
                                "\nSprint - Left Shift" +
                                "\nZoom In / Out - Mousewheel" +
                                "\nToggle 1st/3rd Person View - V" +
                                "\nInteract - Space" +
                                "\nMovie Mode - T" +
                                "\nCall Portal - C";
        }
    }

    public void ShowMouseSensitivitySlider()
    {
        //Disable other UI sections
        if (controlsText.gameObject.activeSelf)
        {
            controlsText.gameObject.SetActive(false);
        }
        if (creditsText.gameObject.activeSelf)
        {
            creditsText.gameObject.SetActive(false);
        }
        if (infoText.gameObject.activeSelf)
        {
            infoText.gameObject.SetActive(false);
        }

        //Toggle Mouse Slider
        if (!mouseSensitivitySlider.gameObject.activeSelf)
        {
            mouseSensitivitySlider.gameObject.SetActive(true);
        }
        else if (mouseSensitivitySlider.gameObject.activeSelf)
        {
            mouseSensitivitySlider.gameObject.SetActive(false);
        }
    }

    public void ShowInfo()
    {
        //Disable other UI sections
        if (controlsText.gameObject.activeSelf)
        {
            controlsText.gameObject.SetActive(false);
        }
        if (mouseSensitivitySlider.gameObject.activeSelf)
        {
            mouseSensitivitySlider.gameObject.SetActive(false);
        }
        if (creditsText.gameObject.activeSelf)
        {
            creditsText.gameObject.SetActive(false);
        }

        //Toggle Info Text
        if (!infoText.gameObject.activeSelf)
        {
            infoText.gameObject.SetActive(true);
        }
        else if (infoText.gameObject.activeSelf)
        {
            infoText.gameObject.SetActive(false);
        }
    }

    public void ShowCredits()
    {
        //Disable other UI sections
        if (controlsText.gameObject.activeSelf)
        {
            controlsText.gameObject.SetActive(false);
        }
        if (mouseSensitivitySlider.gameObject.activeSelf)
        {
            mouseSensitivitySlider.gameObject.SetActive(false);
        }
        if (infoText.gameObject.activeSelf)
        {
            infoText.gameObject.SetActive(false);
        }

        //Toggle Credits Text
        if (!creditsText.gameObject.activeSelf)
        {
            creditsText.gameObject.SetActive(true);
        }
        else if (creditsText.gameObject.activeSelf)
        {
            creditsText.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        musicManager.StopAll();
        quitGame.Play();
        StartCoroutine(QuitGameTimer());
        fadeToBlackVideo.targetCameraAlpha = 0.01f;
        menuCanvas.SetActive(false);
        GameObject.Find("Canvases").SetActive(false);
        quittingGame = true;
    }

    IEnumerator QuitGameTimer()
    {
        yield return new WaitForSeconds(9.7f);
        Application.Quit();
        print("Application Quit");
    }

    void QuitGameZoom()
    {
        if (quittingGame)
        {
            //if (scene2.isLoaded)
            //{
            //    SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            //    SceneManager.UnloadSceneAsync(2);
            //}

            Camera.main.transform.Translate(new Vector3(0, 0.4f, -1f) * Time.deltaTime * 2f);
            Camera.main.transform.RotateAround(player.transform.position, Camera.main.transform.right, Time.deltaTime * 3f);

            fadeToBlackVideo.targetCameraAlpha += fadeToBlackVideo.targetCameraAlpha * Time.deltaTime / 2;
        }
    }

    public void MouseEnter()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            mouseEnter[mouseEnterIndex].Play();
            mouseEnterIndex++;
            if (mouseEnterIndex > 5)
            {
                mouseEnterIndex = 0;
            }
        }
    }

    public void MouseClick()
    {
        mouseClick.Play();
    }

    public void UseMouse()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseActivated = true;
            controllerActivated = false;
        }

    }

    public void UseController()
    {
        if (Input.GetAxis("LS Y") != 0)
        {
            controllerActivated = true;
            mouseActivated = false;
        }
    }

}
