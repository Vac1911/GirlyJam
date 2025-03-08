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

    //Dynamic song information
    public float songPosInBeats;

    public bool musicStarted = false;

    public ScriptUsageTimeline scriptTimeline;

    public event Action<FMOD.Studio.TIMELINE_BEAT_PROPERTIES> OnBeat;

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
            songPosInBeats = ((float)position - firstBeatTime) / msPerBeat;
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
        OnBeat(beat);
    }
}
