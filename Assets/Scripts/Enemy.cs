using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public GameObject player;
    public NavMeshAgent navAgent;

    public float speed;

    public float attackRange;
    public int attackDamage;

    public float attackLength;
    bool isAttacking = false;
    int attackTimer = 0;

    public GameObject healthBarPrefab;
    public GameObject Canvas;
    protected GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        BeatController.Instance.OnBeat += HandleBeat;
        navAgent.speed = speed;
        healthBar = Instantiate(healthBarPrefab, Canvas.transform);
        healthBar.GetComponent<HealthBar>().character = this;
    }

    // Update is called once per frame
    void Update()
    {
        navAgent.destination = player.transform.position;

        if (isAttacking)
        {
            Debug.DrawLine(transform.position, player.transform.position, Color.yellow);
        }
    }

    void HandleBeat(FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat)
    {

        if (!isAttacking)
        {
            // If we arent attackign but are in attack range, start an attack
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < attackRange)
            {
                StartAttack();
            }
        }
        else
        {
            attackTimer++;
            if (attackTimer >= attackLength)
            {
                // If we are attacking and we have waited the attack length, we can now finish the attack
                FinishAttack();
            }
        }
    }

    void StartAttack()
    {
        attackTimer = 0;
        isAttacking = true;
        navAgent.speed = speed / 2f;
    }

    void FinishAttack()
    {
        isAttacking = false;
        navAgent.speed = speed;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < attackRange)
        {
            // If we are still in attack range, send damage
            player.GetComponent<Player>().ReceiveDamage(attackDamage, (int)Mathf.Floor(BeatController.Instance.songPosInBeats));

            Debug.DrawLine(transform.position, player.transform.position, Color.red, 2f);
        }
    }

    public override void ReceiveDamage(int amount, float beat)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(healthBar);
            Destroy(gameObject);

            BeatController.Instance.OnBeat -= HandleBeat;
        }
    }
}
