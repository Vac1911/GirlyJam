using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject player;
    bool hasFired = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasFired && BeatController.Instance.songPosInBeats >= 4)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        hasFired = true;

        Vector3 heading = player.transform.position - transform.position;
        heading = heading.normalized;
        Vector3 position = transform.position + heading;

        GameObject projectileObject = Instantiate(projectilePrefab, position, Quaternion.identity);

        projectileObject.GetComponent<Projectile>().Init(player, 2);
    }
}
