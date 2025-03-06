using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class BeatController : SingletonMB<BeatController>
{

    public event Action<BeatController> OnBeat = beatController => { };

    public AudioSource musicSource;
    public AudioListener audioListener;

    // Static song information
    public int tempo;
    private float secPerBeat;

    //Pause information
    public static bool paused = false;
    public static float pauseTimeStamp = -1f;
    public static float pausedTime = 0;

    //Dynamic song information
    public float songPosition;
    public float songPosInBeats;
    public float dspSongTime;

    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 60f / tempo;
    }

    // Update is called once per frame
    void Update()
    {

        if (paused)
        {
            if (pauseTimeStamp < 0f) // write down the time we paused
            {
                pauseTimeStamp = (float)AudioSettings.dspTime;
                AudioListener.pause = true;
                //Activate some UI here
            }

            return;
        }
        else if (pauseTimeStamp > 0f) // resume if not paused
        {
            AudioListener.pause = false;
            pauseTimeStamp = -1f;
        }

        //calculate the position of the song in seconds from dsp space
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - pausedTime);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;

    }
}
