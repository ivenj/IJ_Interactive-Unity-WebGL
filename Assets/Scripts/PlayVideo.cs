using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class PlayVideo : MonoBehaviour
{
    public GameObject videoCamera;
    public TextMeshProUGUI movieModeText;

    VideoPlayer videoPlayer;
    TextManager textManager;
    WwiseController wwiseController;
    PlayerMovement playerMovement;
    Coroutine resetPlayable;
    Coroutine disableMovieModeText;
    //GameObject[] televisions;
    //string[] tvTags = { "TV_Dustnet", "TV_Swivet" };

    public bool eventPlaying;
    public bool eventPlayable;
    public bool pressedPlay;
    bool movieMode;
    bool movieModeTextBool;
    bool showMovieModeText;     //is variable being used or same as movideModeTextBool??

    void Start()
    {
        eventPlaying = false;
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        textManager = gameObject.GetComponent<TextManager>();
        wwiseController = gameObject.GetComponent<WwiseController>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
    }

    void Update()
    {
        //Play / Stop Video
        //if (InputManager.space && !eventPlaying && eventPlayable && !pressedPlay)
        //{
        //    //StartVideo();
        //    Invoke("StartVideo", 1f);
        //    pressedPlay = true;
        //}
        //else if (InputManager.space && eventPlaying && eventPlayable)
        //{
        //    //StopVideo();
        //    Invoke("StopVideo", 1f);
        //    pressedPlay = true;
        //}

        MovieMode();
    }

    #region TriggerEnterExit
    private void OnTriggerEnter(Collider other)
    {
        eventPlayable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        eventPlayable = false;
        textManager.textTarget.gameObject.SetActive(false);
        movieModeText.gameObject.SetActive(false);
    }
    #endregion

    public void ExternalVideoStart()
    {
        Invoke("StartVideo", 1f);
        pressedPlay = true;
    }

    public void ExternalVideoStop()
    {
        Invoke("StopVideo", 1f);
        pressedPlay = true;
    }

    void StartVideo()
    {
        videoPlayer.Play();
        eventPlaying = true;
        resetPlayable = StartCoroutine(ResetPlayable());
        MoveVideoCamera();
    }   

    public void StopVideo()
    {
        print("Stopped Video");
        videoPlayer.Stop();
        eventPlaying = false;
        pressedPlay = false;
        StopCoroutine(resetPlayable);
    }

    void MovieMode()
    {
        //Change Camera
        if (wwiseController.eventPlaying && InputManager.movieMode && !movieMode)
        {
            videoCamera.SetActive(true);
            movieMode = true;
            movieModeTextBool = true;
            playerMovement.moveAble = false;
            movieModeText.gameObject.SetActive(true);
            disableMovieModeText = StartCoroutine(DisableMovieMoveText());

            if (InputManager.mouseAndKeyboardInput)
            {
                movieModeText.text = "[T] to leave Movie Mode";
            }
            else if (InputManager.joystickInput)
            {
                movieModeText.text = "[Y] to leave Movie Mode";
            }
        }
        else if (InputManager.movieMode && movieMode)
        {
            videoCamera.SetActive(false);
            movieMode = false;
            playerMovement.moveAble = true;
            movieModeText.gameObject.SetActive(true);

            if (InputManager.mouseAndKeyboardInput)
            {
                movieModeText.text = "[T] to enter Movie Mode";
            }
            else if (InputManager.joystickInput)
            {
                movieModeText.text = "[Y] to enter Movie Mode";
            }
        }

        //Hide Interaction Text on Movie Mode
        if (movieMode)
        {
            textManager.textTarget.gameObject.SetActive(false);
        }

        if (wwiseController.eventPlaying && !movieMode)
        {
            movieModeText.gameObject.SetActive(true);

            if (InputManager.mouseAndKeyboardInput)
            {
                movieModeText.text = "[T] to enter Movie Mode";
            }
            else if (InputManager.joystickInput)
            {
                movieModeText.text = "[Y] to enter Movie Mode";
            }
        }
    }

    IEnumerator DisableMovieMoveText()
    {
        yield return new WaitForSeconds(3.5f);
        movieModeText.gameObject.SetActive(false);
        movieModeTextBool = false;
    }

    IEnumerator ResetPlayable()
    {
        print("resetPlayable CoRoutine started");
        float clipLength = (float)videoPlayer.length;
        yield return new WaitForSeconds(clipLength);
        eventPlaying = false;
        pressedPlay = false;
        wwiseController.eventPlaying = false;
        movieModeText.gameObject.SetActive(false);
        movieModeTextBool = false;
    }

    void MoveVideoCamera()
    {
        if (gameObject.CompareTag("TV_Swivet"))
        {
            videoCamera.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            videoCamera.transform.position = new Vector3(transform.position.x, videoCamera.transform.position.y, -18.5f);
        }
        else if (gameObject.CompareTag("TV_Dustnet"))
        {
            videoCamera.transform.position = new Vector3(transform.position.x, videoCamera.transform.position.y, 18.5f);
            videoCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
