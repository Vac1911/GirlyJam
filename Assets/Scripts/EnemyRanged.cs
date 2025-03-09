using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : Character
{
    public GameObject projectilePrefab;
    public GameObject player;
    public NavMeshAgent navAgent;

    public GameObject healthBarPrefab;
    public GameObject Canvas;
    protected GameObject healthBar;


    public int attackIntercal;
    protected int attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        BeatController.Instance.OnBeat += HandleBeat;
        healthBar = Instantiate(healthBarPrefab, Canvas.transform);
        healthBar.GetComponent<HealthBar>().character = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void HandleBeat(FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat)
    {
        attackTimer++;
        if(attackTimer >= attackIntercal)
        {
            FireProjectile();
            attackTimer = 0;
        }
    }

    void FireProjectile()
    {
        Vector3 heading = player.transform.position - transform.position;
        heading = heading.normalized;
        Vector3 position = transform.position + heading;

        GameObject projectileObject = Instantiate(projectilePrefab, position, Quaternion.identity);

        projectileObject.GetComponent<Projectile>().Init(gameObject, player, 4, 10);
    }

    public override void ReceiveDamage(int amount, float beat)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(healthBar);
            Destroy(gameObject);

            BeatController.Instance.OnBeat -= HandleBeat;
        }
    }
}
