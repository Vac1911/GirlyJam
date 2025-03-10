using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(CharacterController))]
public class Player : Character
{
    public float moveSpeed = 5;

    public int attackDamage;
    [Range(0.0f, 0.5f)]
    public float attackThreshold;
    [Range(0.0f, 0.5f)]
    public float dodgeThreshold;
    public float dodgeDistance;

    public float attackTime;
    bool isAttacking = false;

    protected float lastDodge;
    protected Vector3 attackSize = new Vector3(1.5f , 1 , 1.5f);
    private CharacterController controller;
    private Animator anim;

    protected struct DamageEvent {
        public int amount;
        public float beat;
        public DamageEvent(int amount, float beat)
        {
            this.amount = amount;
            this.beat = beat;
        }
    }
    protected List<DamageEvent> damageQueue = new List<DamageEvent>();

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Input.GetButtonDown("Fire1"))
            Attack();
        if (Input.GetButtonDown("Jump"))
            Dodge();

        TakeDamage();
    }

    void Attack()
    {
        if (isAttacking) return;

        float currentBeat = BeatController.Instance.songPosInBeats;

        float offBeat = currentBeat - Mathf.Round(currentBeat);
        BeatFeedback.Instance.SendFeedback(this, offBeat, attackThreshold);

        // Are we attacking at the right time?
        if (Mathf.Abs(offBeat) > attackThreshold)
        {
            Debug.Log("attack failed " + offBeat);
            return;
        }

        Debug.Log("attack success");

        // Get the direction we are trying to attack
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 startPosition = transform.position;
        Plane m_Plane = new Plane(Vector3.up, -1);
        float enter;
        if (!m_Plane.Raycast(ray, out enter))
            Debug.LogError("Invalid Raycast to Plane");

        Vector3 hitPoint = ray.GetPoint(enter);
        Vector3 heading = hitPoint - startPosition;
        heading.Normalize();


        // Get objects hit by our attack
        Collider[] hitColliders = Physics.OverlapBox(transform.position + heading * attackSize.z, attackSize / 2, Quaternion.LookRotation(heading));
        int i = 0;
        while (i < hitColliders.Length)
        {
            Collider collider = hitColliders[i];
            Character hitCharacter = collider.gameObject.GetComponent<Character>();
            if(hitCharacter)
            {
                Debug.Log("Hit : " + collider.name);
                hitCharacter.ReceiveDamage(attackDamage, currentBeat);
            }

            i++;
        }

        transform.rotation = Quaternion.LookRotation(heading);

        // Start Attack Animation;
        isAttacking = true;
        anim.SetTrigger("IsAttacking");
        StartCoroutine("DoAttackAnim");
    }

    IEnumerator DoAttackAnim()
    {
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
    }

    void Dodge()
    {
        lastDodge = BeatController.Instance.songPosInBeats;
        float offBeat = lastDodge - Mathf.Round(lastDodge);
        BeatFeedback.Instance.SendFeedback(this, offBeat, dodgeThreshold);
        if (Mathf.Abs(offBeat) < dodgeThreshold)
        {
            Debug.Log("Dodge Success " + offBeat);

            controller.Move(GetMoveDirection() * dodgeDistance);
        }
        else
        {
            Debug.Log("Dodge Fail " + offBeat);
        }
    }
    void Move()
    {
        float speed = isAttacking ? moveSpeed / 2 : moveSpeed;
        Vector3 direction = GetMoveDirection();
        Vector3 motion = direction * speed * Time.deltaTime;
        controller.Move(motion);

        anim.SetBool("IsMoving", true);
        if(!isAttacking)
        {

            transform.rotation = direction != Vector3.zero ? Quaternion.LookRotation(GetMoveDirection()) : Quaternion.identity;
        }
    }

    Vector3 GetMoveDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        return new Vector3(horizontalInput, 0, verticalInput);
    }

    public override void ReceiveDamage(int amount, float beat)
    {
        // Damage is qued incase players dodge late but still within an acceptable time
        damageQueue.Add(new DamageEvent(amount, beat));
    }

    void TakeDamage()
    {
        float currentBeat = BeatController.Instance.songPosInBeats;
        foreach (DamageEvent damageEvent in damageQueue.ToArray())
        {
            // If it is too late for the player to dodge successfully
            if(currentBeat > damageEvent.beat + dodgeThreshold)
            {
                float lastDodgeOffBeat = Mathf.Abs(lastDodge - damageEvent.beat);
                if (lastDodgeOffBeat < dodgeThreshold)
                {
                    Debug.Log("dodged");
                }
                else
                {
                    Debug.Log("hit");
                }
                damageQueue.Remove(damageEvent);
            }
        }
    }
}
