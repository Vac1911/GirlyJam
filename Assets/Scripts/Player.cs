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

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // Cast a ray from screen point
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.point);
            Vector3 heading = hit.point - transform.position;
            heading.y = 0;
            heading.Normalize();
            Debug.DrawLine(transform.position + heading * 0.75f, transform.position + heading * 2, Color.blue, 10f);
        }
        /*Plane m_Plane = new Plane(Vector3.up, 1);
        float enter = 0.0f;
        if (m_Plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Debug.Log(hitPoint);
            Vector3 heading = hitPoint - transform.position;
            heading.y = 0;
            heading.Normalize();
            Debug.DrawLine(transform.position + heading * 0.75f, transform.position + heading * 2, Color.blue, 10f);
        }*/


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
