using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public abstract void ReceiveDamage(int amount, float beat);
}
