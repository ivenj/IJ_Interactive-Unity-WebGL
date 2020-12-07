using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class WwiseController : MonoBehaviour
{
    public AudioSource wwiseEvent;

    public string eventName;
    public float eventLength;
    public bool isLoop;
    public bool isOneShot;
    //public bool isStoppable = true;
    public bool eventPlaying;


    Animator playerAnim;
    public Coroutine resetEventPlaying;
    Coroutine stopEventText;
    GameObject[] musicalMoments;
    GameObject[] televisions;
    MusicManager musicManager;
    PlayVideo playVideo;
    TextManager textManager;
    VideoPlayer videoPlayer;

    string[] objectTags = { "Tablet_MM", "TV_Dustnet", "TV_Swivet" };
    string[] tvTags = { "TV_Dustnet", "TV_Swivet" };
    bool pressedPlay;
    bool eventPlayable;
    uint eventUInt;
    uint stopEvent;

    void Awake()
    {
        if (eventName.Length == 0)
        {
            eventName = "TEST";
        }
    }

    void Start()
    {
        wwiseEvent = GetComponent<AudioSource>();
        textManager = GetComponent<TextManager>();
        playerAnim = GameObject.Find("Android").GetComponent<Animator>();
        eventPlaying = false;
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    void Update()
    {
        if (InputManager.space && isOneShot && eventPlayable)
        {
            wwiseEvent.Play();
        }
        else if (InputManager.space && !eventPlaying && eventPlayable && !isOneShot && !pressedPlay)
        {
            musicManager.StopAll();
            Invoke("PostEvent", 1f);
            pressedPlay = true;
            playerAnim.SetBool("pressedPlay", pressedPlay);

            //Trigger Video on TVs
            if (gameObject.tag == "TV_Dustnet" || gameObject.tag == "TV_Swivet")
            {
                playVideo = GetComponent<PlayVideo>();
                playVideo.ExternalVideoStart();
            }
        }
        else if (InputManager.space && eventPlaying && eventPlayable && !isOneShot && !pressedPlay)
        {
            Invoke("StopEvent", 1f);
            pressedPlay = true;
            playerAnim.SetBool("pressedPlay", pressedPlay);

            //Stop Video on TVs
            if (gameObject.tag == "TV_Dustnet" || gameObject.tag == "TV_Swivet")
            {
                playVideo.ExternalVideoStop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        eventPlayable = true;
    }
    private void OnTriggerExit(Collider other)
    {
        eventPlayable = false;
    }

    public void PostEvent()
    {
        //stopping other events & resetting text and playability
        foreach (string tag in objectTags)
        {
            musicalMoments = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject _musicalMoment in musicalMoments)
            {
                if (InputManager.mouseAndKeyboardInput && !InputManager.joystickInput)
                    _musicalMoment.GetComponent<TextManager>().textTarget.text = "[Space] to play '" + _musicalMoment.GetComponent<WwiseController>().eventName + "'";
                else if (InputManager.joystickInput && !InputManager.mouseAndKeyboardInput)
                    _musicalMoment.GetComponent<TextManager>().textTarget.text = "[X] to play '" + _musicalMoment.GetComponent<WwiseController>().eventName + "'";

                _musicalMoment.GetComponent<WwiseController>().eventPlaying = false;
            }
        }

        //stopping video & resetting bools
        foreach (string tag in tvTags)
        {
            televisions = GameObject.FindGameObjectsWithTag(tag);

            for (int i = 0; i < televisions.Length; i++)
            {
                televisions[i].GetComponent<VideoPlayer>().Stop();
                televisions[i].GetComponent<PlayVideo>().eventPlaying = false;
                televisions[i].GetComponent<PlayVideo>().pressedPlay = false;
                televisions[i].GetComponent<PlayVideo>().movieModeText.gameObject.SetActive(false);
            }
        }

        //posting the actual event
        wwiseEvent.Play();

        pressedPlay = false;
        playerAnim.SetBool("pressedPlay", pressedPlay);

        stopEventText = StartCoroutine(SetEventPlaying());

        if (eventLength != 0)
        {
            resetEventPlaying = StartCoroutine(EventPlaying());
        }
        else
        {

        }

        if (this.gameObject.CompareTag("Tablet_MM") || this.gameObject.CompareTag("TV_Dustnet") || this.gameObject.CompareTag("TV_Swivet"))
        {
            if (InputManager.mouseAndKeyboardInput)
                textManager.textTarget.text = "[Space] to stop '" + eventName + "'";
            else if (InputManager.joystickInput)
                textManager.textTarget.text = "[X] to stop '" + eventName + "'";
        }
    }

    public void StopEvent()
    {
        //STOP AUDIO FILE
        musicManager.StopAllSlowly();

        eventPlaying = false;

        pressedPlay = false;
        playerAnim.SetBool("pressedPlay", pressedPlay);

        if (eventLength != 0)
        {
            StopCoroutine(resetEventPlaying);
        }

        if (gameObject.CompareTag("Tablet_MM") || gameObject.CompareTag("TV_Dustnet") || gameObject.CompareTag("TV_Swivet"))
        {
            stopEventText = StartCoroutine(StopEventText());
        }
    }

    IEnumerator EventPlaying()
    {
        yield return new WaitForSeconds(eventLength);
        eventPlaying = false;
        if (InputManager.mouseAndKeyboardInput)
            textManager.textTarget.text = "[Space] to play '" + eventName + "'";
        else if (InputManager.joystickInput)
            textManager.textTarget.text = "[X] to play '" + eventName + "'";
    }

    IEnumerator StopEventText()
    {
        if (gameObject.CompareTag("Tablet_MM"))
        {
            textManager.textTarget.text = "Stopping.";
            yield return new WaitForSeconds(1);
            textManager.textTarget.text = "Stopping..";
            yield return new WaitForSeconds(1);
            textManager.textTarget.text = "Stopping...";
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForEndOfFrame();

        if (InputManager.mouseAndKeyboardInput)
            textManager.textTarget.text = "[Space] to play '" + eventName + "'";
        else if (InputManager.joystickInput)
            textManager.textTarget.text = "[X] to play '" + eventName + "'";
    }

    IEnumerator SetEventPlaying()
    {
        yield return new WaitForSeconds(0.05f);
        eventPlaying = true;

    }
}
