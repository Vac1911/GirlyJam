using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int startBeat;
    public int duration;
    private GameObject player;
    protected float initialDistance;

    public int endBeat
    {
        get => startBeat + duration;
    }

    public static GameObject template;

    public void Init(GameObject player, int duration)
    {
        this.startBeat = (int)Mathf.Floor(BeatController.Instance.songPosInBeats);
        this.player = player;
        this.duration = duration;
        this.initialDistance = Vector3.Distance(transform.position , player.transform.position);
    }

    private void Update()
    {
        float currentBeat = BeatController.Instance.songPosInBeats;
        float currentPercent = (currentBeat - (float)startBeat) / (float)duration;
        float distanceToTarget = (1f - currentPercent) * initialDistance;

        Vector3 heading = transform.position - player.transform.position;
        heading = heading.normalized;
        Vector3 position = player.transform.position + heading * distanceToTarget;

        transform.position = position;

        if (currentPercent > 1)
        {
            // Hit the player
            Destroy(gameObject);

        }
    }
}
