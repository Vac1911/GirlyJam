using Patterns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BeatFeedback : SingletonMB<BeatFeedback>
{
    public Color perfectColor;
    public Color goodColor;
    public Color missColor;

    TextMeshProUGUI textMeshPro;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendFeedback(Player player, float offBeat, float threshold)
    {
        if (BeatController.Instance.IsPerfect(offBeat))
        {
            textMeshPro.color = perfectColor;
            textMeshPro.SetText("Perfect");
        }
        else if(Mathf.Abs(offBeat) < threshold)
        {
            textMeshPro.color = goodColor;
            textMeshPro.SetText("Good");
        }
        else if(offBeat > 0)
        {
            textMeshPro.color = missColor;
            textMeshPro.SetText("Late");
        }
        else
        {
            textMeshPro.color = missColor;
            textMeshPro.SetText("Early");
        }
    }
}
