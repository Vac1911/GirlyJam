using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int startBeat;
    public int duration;
    public int damage;
    private GameObject sender;
    private GameObject target;

    public int endBeat
    {
        get => startBeat + duration;
    }

    public static GameObject template;

    public void Init(GameObject sender, GameObject target, int duration, int damage)
    {
        this.damage = damage;
        this.startBeat = (int)Mathf.Floor(BeatController.Instance.songPosInBeats);
        this.sender = sender;
        this.target = target;
        this.duration = duration;
    }

    private void Update()
    {
        float currentBeat = BeatController.Instance.songPosInBeats;
        float currentPercent = (currentBeat - (float)startBeat) / (float)duration;

        if(sender == null)
        {
            Destroy(gameObject);
            return;
        }
        float totalDistance = Vector3.Distance(sender.transform.position, target.transform.position);
        float currentDistance = currentPercent * totalDistance;

        Vector3 heading = sender.transform.position - target.transform.position;
        heading = heading.normalized;

        Vector3 position = sender.transform.position - heading * currentDistance;
        transform.position = position;

        if (currentPercent > 1)
        {
            target.GetComponent<Character>().ReceiveDamage(damage, endBeat);
            Destroy(gameObject);
        }
    }
}
