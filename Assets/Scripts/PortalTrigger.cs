using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalTrigger : MonoBehaviour
{
    #region VariableDeclaration

    public float turnSpeed;
    public bool isFlyingDown;
    public bool portalMovedToCenter;
    public bool portalMovedBack;
    public TextMeshProUGUI portalText;
    public AudioSource portalTriggerEvent_1;
    public AudioSource portalTriggerEvent_2;
    public AudioSource portalFlyEvent_1;
    public AudioSource portalFlyEvent_2;
    public AudioSource raisePortalEvent;

    bool triggerEnabled;
    bool triggerProximity;
    bool portalCallable;
    bool portalBackInPlace;
    bool isFlyingUp;
    bool platformRaisable = true;
    bool raisePlatform;
    bool pressedPlay;     
    int flyDirection = 0;
    
    Transform playerPosition;
    PlayerMovement playerMovement;
    GameObject portalSphere;
    PortalScript portalScript;
    Coroutine movePortalSphereBackCoRoutine;
    Coroutine platformRaisableCoRoutine;
    Rigidbody playerRb;
    Animator playerAnim;
    PlayVideo[] videoPlayers;

    #endregion

    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        portalSphere = GameObject.Find("Portal Sphere");
        portalScript = GameObject.Find("Portal Sphere").GetComponent<PortalScript>();
        playerAnim = GameObject.Find("Android").GetComponent<Animator>();
        isFlyingDown = false;
        videoPlayers = GameObject.FindObjectsOfType<PlayVideo>();
    }

    void Update()
    {
        TextDisplay();
        CallPortalBack();

        if (raisePlatform)
            RaisePortalPlatform();
        else if (!raisePlatform)
            LowerPortalPlatform();
    }

    private void FixedUpdate()
    {
        EnterPortal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (platformRaisable)
        {
            raisePortalEvent.Play();
            raisePlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        raisePlatform = false;
    }

    public void TriggerEnter()
    {
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded || (SceneManager.GetSceneByBuildIndex(2).isLoaded && portalBackInPlace))
        {
            triggerProximity = true;
        }

        if (SceneManager.GetSceneByBuildIndex(2).isLoaded && !portalBackInPlace)
        {
            portalCallable = true;
        }

        //disable Trigger Count
        if (triggerEnabled)
        {
            triggerEnabled = false;
        }
    }

    public void TriggerExit()
    {
        triggerProximity = false;

        if (portalCallable)
        {
            platformRaisableCoRoutine = StartCoroutine(PlatformRaisable());
        }

        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            platformRaisable = true;
    }


    void TextDisplay()
    {
        //Text Display
        if (triggerProximity && !portalCallable)
        {
            if (InputManager.mouseAndKeyboardInput)
            {
                portalText.text = "[Space]";
            }
            else if (InputManager.joystickInput)
            {
                portalText.text = "[X]";
            }
        }
        else if (!triggerProximity && portalCallable && !portalMovedToCenter)
        {
            portalText.text = "";
        }
    }
    
    void EnterPortal()
    {
        //Levitate into Portal
        if (triggerProximity && !portalCallable)
        {
            if (InputManager.space)
            {
                PressedPlayToggle();
                playerRb.useGravity = false;
                Invoke("Flying", 1f);
                Invoke("PressedPlayToggle", .5f);

                foreach (PlayVideo p in videoPlayers)
                {
                    p.movieModeText.gameObject.SetActive(false);
                }
            }
        }
        else if (isFlyingUp && playerPosition.position.y >= 6f)
        {
            flyDirection = 0;
        }

        //Constrict Player Movement when Flying
        if (isFlyingUp || isFlyingDown)
        {
            playerMovement.moveAble = false;
        }
        else
        {
            playerMovement.moveAble = true;
        }


        //Fly Up
        if (isFlyingUp)
        {
            playerRb.velocity = Vector3.up * Time.deltaTime * flyDirection * 50;
        }

        if (isFlyingUp && playerMovement.mainCameraVector.magnitude <= 12.5f)
        {
            playerMovement.mainCamera.transform.Translate(new Vector3(0, 0, -1f) * Time.deltaTime);
        }
        if (isFlyingUp && Camera.main.transform.localEulerAngles.x < 26f || isFlyingUp && Camera.main.transform.localEulerAngles.x > 340f)
        {
            Camera.main.transform.RotateAround(playerMovement.transform.position, Camera.main.transform.right, Time.deltaTime * 3);
        }
        if (isFlyingUp && Camera.main.transform.localEulerAngles.x > 27f && Camera.main.transform.localEulerAngles.x < 70f)
        {
            Camera.main.transform.RotateAround(playerMovement.transform.position, -Camera.main.transform.right, Time.deltaTime * 3);
        }

        //Stop turning when facing forward
        if (isFlyingUp && playerPosition.transform.eulerAngles.y < 359.5f && playerPosition.position.y > 1.3f)
        {
            playerPosition.transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed);
        }

        //Fly Down
        if (isFlyingDown)
        {
            platformRaisable = false;
            isFlyingUp = false;

            //Move Player To Ground and Zoom Back In
            if (playerPosition.position.y >= 1.3f)
            {
                playerRb.velocity = Vector3.down * Time.deltaTime * 50;

                playerMovement.mainCamera.transform.Translate(new Vector3(0, 0, 1.5f) * Time.deltaTime);

                if (Camera.main.transform.localEulerAngles.x > -1f)
                {
                    Camera.main.transform.RotateAround(playerMovement.transform.position, -Camera.main.transform.right, Time.deltaTime * 6);
                }
            }
            else
            {
                isFlyingDown = false;
                playerRb.useGravity = true;
            }
        }
    }

    void Flying()
    {
        flyDirection = 1;
        isFlyingUp = true;
        triggerEnabled = true;
        raisePlatform = false;

        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            portalTriggerEvent_1.Play();
            portalFlyEvent_1.Play();
        }
        else if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            portalTriggerEvent_2.Play();
            portalFlyEvent_2.Play();
        }
    }

    void CallPortalBack()
    {
        //Call Portal Back
        if (InputManager.callPortal && portalCallable)
        {
            movePortalSphereBackCoRoutine = StartCoroutine(portalScript.MovePortalSphereBackCoRoutine());

            PressedPlayToggle();                
            Invoke("PressedPlayToggle", .5f);
            Invoke("PortalTriggerEvent", 1f);
        }

        //Portal Back In Place
        if (portalSphere.transform.position.y < 6.5f && portalSphere.transform.position.z < -10.5f && portalMovedBack)
        {
            portalBackInPlace = true;
            portalCallable = false;
            portalMovedToCenter = false;
            StopCoroutine(movePortalSphereBackCoRoutine);
        }

        if (portalSphere.transform.position.y > 7.5f && portalSphere.transform.position.z > -9.5f)
        {
            portalBackInPlace = false;
        }
    }

    void RaisePortalPlatform()
    {

        if (transform.position.y < 0.3f && platformRaisable)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 0.2f);
        }

    }

    void LowerPortalPlatform()
    {
        if (transform.position.y > 0.2f && platformRaisable)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 0.2f);
        }
    }

    IEnumerator PlatformRaisable()
    {
        yield return new WaitForSeconds(5);
        platformRaisable = true;

        if (InputManager.mouseAndKeyboardInput)
        {
            portalText.text = "[C] to call Portal";
        }
        else if (InputManager.joystickInput)
        {
            portalText.text = "[B] to call Portal";
        }
    }

    void PressedPlayToggle()
    {
        pressedPlay = !pressedPlay;
        playerAnim.SetBool("pressedPlay", pressedPlay);
    }

    void PortalTriggerEvent()
    {
        portalTriggerEvent_1.Play();
    }
}
