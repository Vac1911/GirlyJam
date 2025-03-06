using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 10;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Input.GetButtonDown("Fire1"))
            Attack();

    }

    void Attack()
    {
        float beat = BeatController.Instance.songPosInBeats;
        Debug.Log(beat);
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 motion = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;
        controller.Move(motion);
    }
}
