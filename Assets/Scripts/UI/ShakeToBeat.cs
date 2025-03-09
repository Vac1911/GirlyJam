using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ShakeToBeat : MonoBehaviour
{
    public Vector3 offset;
    public AnimationCurve curve;
    protected Vector3 startPos;
    protected RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float beat = BeatController.Instance.songPosInBeats;
        float beatProgress = beat - Mathf.Floor(beat);
        rectTransform.localPosition = Vector3.Lerp(startPos, startPos + offset, curve.Evaluate(beatProgress));
    }
}
