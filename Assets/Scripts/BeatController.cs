using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(ScriptUsageTimeline))]
public class BeatController : SingletonMB<BeatController>
{
    // Static song information
    private int tempo = 0;
    private float secPerBeat;

    //Pause information
    public GameObject PauseCanvas;

    //Dynamic song information
    public float songPosition;
    public float songPosInBeats;
    public float dspSongTime;

    public bool musicStarted = false;

    public ScriptUsageTimeline scriptTimeline;

    // Start is called before the first frame update
    void Start()
    {
        ScriptUsageTimeline scriptTimeline = GetComponent<ScriptUsageTimeline>();
        songPosition = -songPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo > 0)
        {
            int position = scriptTimeline.GetTimelinePosition();
            Debug.Log(position);

            /*currentTime = musicInstance.getTimelinePosition
            //calculate the position of the song in seconds from dsp space
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);

            //calculate the position in beats
            songPosInBeats = songPosition / secPerBeat;*/
        }

    }

    /*public void ReadFirstBeat(FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat)
    {
        tempo = (int)beat.tempo;
        secPerBeat = 60f / tempo
    }*/
}
