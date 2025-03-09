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
    private int firstBeatTime;
    private int msPerBeat;

    public int beatsPerLoop = 0;

    //Dynamic song information
    public float songPosInBeats;
    public int loopCounter = 0;

    public bool musicStarted = false;

    public ScriptUsageTimeline scriptTimeline;

    public event Action<FMOD.Studio.TIMELINE_BEAT_PROPERTIES>? OnBeat;

    // Start is called before the first frame update
    void Start()
    {
        ScriptUsageTimeline scriptTimeline = GetComponent<ScriptUsageTimeline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicStarted)
        {
            int position = scriptTimeline.GetTimelinePosition();
            float nextSongPosInBeats = (((float)position - firstBeatTime) / msPerBeat) + loopCounter * beatsPerLoop;
            if (nextSongPosInBeats < songPosInBeats)
            {
                if(beatsPerLoop == 0)
                {
                    beatsPerLoop = Mathf.CeilToInt(songPosInBeats);
                }
                loopCounter++;
            }
            songPosInBeats = nextSongPosInBeats;
        }
    }

    public void ReadFirstBeat(FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat)
    {
        if (!musicStarted)
        {
            musicStarted = true;
            tempo = (int)beat.tempo;
            msPerBeat = 60000 / tempo;
            firstBeatTime = beat.position;
        }
        if(OnBeat != null) OnBeat(beat);
    }
    /**
     * FMOD getTimelinePosition is dependent on "Studio update rate" it only has an accuracy of 20ms.
     * So anything within 20ms should be counted as perfect.
     * @see https://qa.fmod.com/t/gettimelineposition-accuracy-for-rhythm-game/20202
     */
    public bool IsPerfect(float offBeat)
    {
        return Mathf.Abs(offBeat) < 0.020f;
    }
}
