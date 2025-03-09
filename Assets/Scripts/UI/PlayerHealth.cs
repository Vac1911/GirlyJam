using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Player player;
    public RawImage fill;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = (float)player.health / player.maxHealth;
        fill.rectTransform.localScale = new Vector3(percentage, percentage, percentage);
    }
}
