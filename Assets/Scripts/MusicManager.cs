﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public WwiseController[] musicTracks;

    public float fadeTime;
    public float resetTime;
    public float stopTime;
    public float targetVolume;

    string targetParameter = "MusicVolume";

    void Start()
    {

    }

    void Update()
    {
        SFXSideChainCompression();
    }

    public void StopAll()
    {
        foreach (WwiseController musicTrack in musicTracks)
        {
            if (musicTrack.resetEventPlaying != null)
            {
                StartCoroutine(FadeMixerGroup.StartFade(mainAudioMixer, targetParameter, fadeTime, targetVolume));
                Invoke("StopEvents", stopTime);
                Invoke("ResetVolume", resetTime);
            }
        }

    }

    public void StopAllSlowly()
    {
        foreach (WwiseController musicTrack in musicTracks)
        {
            if (musicTrack.resetEventPlaying != null)
            {
                StartCoroutine(FadeMixerGroup.StartFade(mainAudioMixer, targetParameter, 3, targetVolume));
                Invoke("StopEvents", 3.0f);
                Invoke("ResetVolume", 3.5f);
            }
        }

    }

    public void StopEvents()
    {
        foreach (WwiseController musicTrack in musicTracks)
        {
            musicTrack.wwiseEvent.Stop();
        }
    }

    public void ResetVolume()
    {
        mainAudioMixer.SetFloat(targetParameter, 0);
    }

    public void SFXSideChainCompression()
    {

        //if ()
        //{
        //    StartCoroutine(FadeMixerGroup.StartFade(mainAudioMixer, "SFXVolume", 3, -12));
        //}
    }
}
