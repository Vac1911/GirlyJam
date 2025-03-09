using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HealthBar : MonoBehaviour
{
    public Character character;
    public RawImage bar;
    public RawImage fill;

    public float width;
    public float height;

    // Start is called before the first frame update
    void Start()
    {
        width = bar.rectTransform.sizeDelta.x;
        height = bar.rectTransform.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = (float)character.health / character.maxHealth;
        fill.rectTransform.localScale = new Vector3(percentage, 1, 1);
        transform.position = Camera.main.WorldToScreenPoint(character.transform.position);
    }
}
