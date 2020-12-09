using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Movement Variables
    public float movementSpeedZ;
    public float movementSpeedX;
    public float turnSpeed;
    public bool moveAble = true;
    public bool lookAble = true;

    float _movementSpeedZ;
    float _movementSpeedX;
    float verticalInput;
    float horizontalInput;
    float levelBoundaries;
    bool runIsToggled;

    //Mouse Variables
    public Slider mouseSensitivitySlider;
    public float mouseSensitivity;
    public float mouseYLimitUp;
    public float mouseYLimitDown;
    public float scrollSpeed;

    float _mouseSensitivity;
    float mouseWheelInput;
    float mouseXInput;
    float mouseYInput;

    float joystickSensibility = .2f;

    //Camera Variables
    public float cameraYOffset;
    public Vector3 mainCameraVector;
    public Camera mainCamera;


    Vector3 currentCameraPosition;
    float currentCameraDistance;
    float mainCameraAngleX;
    float mainCameraAngleXClamped;
    bool firstPersonView;

    //Misc Variables
    public GameManager gameManager;
    public GameObject portalTrigger;
    public PortalScript portalScript;

    PortalTrigger _portalTrigger;
    Rigidbody playerRb;
    SkinnedMeshRenderer playerMesh1;
    SkinnedMeshRenderer playerMesh2;

    Animator playerAnim;
    public bool isWalkingForward;
    public bool isWalkingBackward;
    public bool isRunningForward;
    public bool isRunningBackward;
    public bool isTurningLeft;
    public bool isTurningRight;
    bool pressedPlay;

    void Start()
    {
        mainCamera = Camera.main;
        _movementSpeedZ = movementSpeedZ;
        _movementSpeedX = movementSpeedX;
        playerRb = GetComponent<Rigidbody>();
        _portalTrigger = portalTrigger.GetComponent<PortalTrigger>();
        playerAnim = GameObject.Find("Android").GetComponent<Animator>();
        playerMesh1 = GameObject.Find("Beta_Joints").GetComponent<SkinnedMeshRenderer>();
        playerMesh2 = GameObject.Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        RunToggle();
        MouseCountrols();
        ConstrainPlayerMovement();
        AnimationControls();
    }

    private void FixedUpdate()
    {
        KeyboardControls();
    }

    private void LateUpdate()
    {
        CameraFunctionality();
    }



    void KeyboardControls()
    {
        //Horizontal Input - Keyboard + Joystick
        if (Input.GetAxis("RS X") > joystickSensibility && Input.GetAxis("Horizontal") > joystickSensibility ||
                Input.GetAxis("RS X") < -joystickSensibility && Input.GetAxis("Horizontal") < -joystickSensibility)
            horizontalInput = (Input.GetAxis("Horizontal") + Input.GetAxis("RS X")) * .75f;
        else if (Input.GetAxis("Horizontal") > joystickSensibility || Input.GetAxis("Horizontal") < -joystickSensibility)
            horizontalInput = Input.GetAxis("Horizontal");
        else if (Input.GetAxis("RS X") > joystickSensibility || Input.GetAxis("RS X") < -joystickSensibility)
            horizontalInput = Input.GetAxis("RS X");
        else
            horizontalInput = 0f;

        //Vertical Input
        verticalInput = Input.GetAxis("Vertical");

        if (moveAble)
        {
            playerRb.velocity = transform.forward * _movementSpeedZ * Time.deltaTime * verticalInput;
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * horizontalInput);
        }
    }

    void RunToggle()
    {
        if (InputManager.shift)
        {
            _movementSpeedZ = movementSpeedZ * 2;
            _movementSpeedX = movementSpeedX * 2;
            runIsToggled = true;
        }
        else if (!InputManager.shift)
        {
            _movementSpeedZ = movementSpeedZ;
            _movementSpeedX = movementSpeedX;
            runIsToggled = false;
        }
    }

    void MouseCountrols()
    {
        //ZOOM - Mousewheel or Joystick 
        if (Input.GetKey(KeyCode.JoystickButton8))
            mouseWheelInput = Input.GetAxis("RS Y") * .1f;
        else
            mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");


        //X+Y - Mouse or Joystick 
        mouseXInput = Input.GetAxis("Mouse X");

        if (Input.GetAxis("Mouse Y") != 0)
            mouseYInput = Input.GetAxis("Mouse Y");
        else if (!Input.GetKey(KeyCode.JoystickButton8))
            mouseYInput = Input.GetAxis("RS Y");


        _mouseSensitivity = mouseSensitivity * mouseSensitivitySlider.value;

        //Mouse X&Y Controls
        mainCameraAngleX = mainCamera.transform.localEulerAngles.x;

        if (mainCameraAngleX < 100)
        {
            mainCameraAngleXClamped = mainCamera.transform.localEulerAngles.x;
        }
        else if (mainCameraAngleX > 300)
        {
            mainCameraAngleXClamped = mainCamera.transform.localEulerAngles.x - 360f;
        }

        if (moveAble)
        {
            //Vertical Mouse Movement + Limiting Y Movement
            if (mainCameraVector.magnitude >= 2f)
            {
                if ((mainCameraAngleXClamped < 60 && mouseYInput < 0) || ((mainCameraAngleXClamped) > -6 && mouseYInput > 0))
                {
                    mainCamera.transform.RotateAround(transform.position, -mainCamera.transform.right, _mouseSensitivity * Time.deltaTime * mouseYInput);
                }
            }
            //Different angles for First Person
            else if (mainCameraVector.magnitude < 2f)
            {
                if ((mainCameraAngleXClamped < 45 && mouseYInput < 0) || ((mainCameraAngleXClamped) > -32 && mouseYInput > 0))
                {
                    mainCamera.transform.RotateAround(transform.position, -mainCamera.transform.right, _mouseSensitivity * Time.deltaTime * mouseYInput);
                }
            }



            //if (true) //(Input.GetMouseButton(0) && !_firstPersonOn)
            //{
            //    mainCamera.transform.RotateAround(transform.position, transform.up, _mouseSensitivity * Time.deltaTime * mouseXInput);
            //}

            if (true)
            {
                //Horizontal Mouse Movement
                transform.RotateAround(transform.position, transform.up, _mouseSensitivity * Time.deltaTime * mouseXInput);
            }
        }

    }

    void CameraFunctionality()
    {
        //ScrollWheel Zoom
        mainCameraVector = mainCamera.transform.position - transform.position;

        if (moveAble)
        {
            if (mainCameraVector.magnitude >= 9f && mouseWheelInput < 0)
            { //do nothing
            }
            else if (mainCameraVector.magnitude <= 4f && mouseWheelInput > 0)
            { //do nothing
            }
            else
            {
                mainCamera.transform.Translate(Vector3.forward * scrollSpeed * Time.deltaTime * mouseWheelInput);
            }
        }

        //First Person View with 'F'
        if (InputManager.firstPerson && transform.position.y < 1.5f && !firstPersonView)
        {
            currentCameraDistance = (transform.position - mainCamera.transform.position).magnitude;

            //transform.position.z - mainCamera.transform.position.z;

            mainCamera.transform.position = transform.position + new Vector3(0, 1.2f, 0.4f);
            firstPersonView = true;
        }
        else if (firstPersonView && InputManager.firstPerson || firstPersonView && mouseWheelInput < 0)
        {
            mainCamera.transform.position = mainCamera.transform.position - mainCamera.transform.forward * currentCameraDistance;
            firstPersonView = false;
        }


        if ((transform.position - mainCamera.transform.position).magnitude > 1)
        {
            playerMesh1.gameObject.SetActive(true);
            playerMesh2.gameObject.SetActive(true);
        }

        else if ((transform.position - mainCamera.transform.position).magnitude < 1)
        {
            playerMesh1.gameObject.SetActive(false);
            playerMesh2.gameObject.SetActive(false);
        }
    }

    void ConstrainPlayerMovement()
    {
        //Change boundary depending on Scene
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            if (portalScript.scenesLoaded == 1) 
                levelBoundaries = 12.5f;
            else if (portalScript.scenesLoaded > 1)
                levelBoundaries = 24.9f;
        }
        else if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            levelBoundaries = 23.5f;
        }

        //Check for each boundary
        if (transform.position.x > levelBoundaries)
        {
            transform.position = new Vector3(levelBoundaries, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -levelBoundaries)
        {
            transform.position = new Vector3(-levelBoundaries, transform.position.y, transform.position.z);
        }
        if (transform.position.z > levelBoundaries)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, levelBoundaries);
        }
        if (transform.position.z < -levelBoundaries)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -levelBoundaries);
        }
    }

    void AnimationControls()
    {
        if (moveAble)
        {
            if (verticalInput > 0)
                isWalkingForward = true;
            else
                isWalkingForward = false;

            if (verticalInput < 0)
                isWalkingBackward = true;
            else
                isWalkingBackward = false;

            if (verticalInput > 0 && runIsToggled)
                isRunningForward = true;
            else
                isRunningForward = false;

            if (verticalInput < 0 && runIsToggled)
                isRunningBackward = true;
            else
                isRunningBackward = false;

            if (horizontalInput > joystickSensibility && verticalInput == 0)
                isTurningLeft = true;
            else
                isTurningLeft = false;

            if (horizontalInput < -joystickSensibility && verticalInput == 0)
                isTurningRight = true;
            else
                isTurningRight = false;
        }

        //Animator
        playerAnim.SetBool("isWalkingForward", isWalkingForward);
        playerAnim.SetBool("isWalkingBackward", isWalkingBackward);
        playerAnim.SetBool("isRunningForward", isRunningForward);
        playerAnim.SetBool("isRunningBackward", isRunningBackward);
        playerAnim.SetBool("isTurningLeft", isTurningLeft);
        playerAnim.SetBool("isTurningRight", isTurningRight);
    }
}
