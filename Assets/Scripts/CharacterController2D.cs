using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 2f;
    Vector2 motionVector;
    public Vector2 lastMotionVector;
    Animator animator;
    public bool moving;
    //Dash
    [SerializeField] float dashingPower = 10f;
    private bool dashing = false;
    private bool canDash = true;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;



    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector.x = horizontal;
        motionVector.y = vertical;

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            dashing = true;
            animator.SetTrigger("dash");
        }

        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;

            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }

    void FixedUpdate()
    {
        if (!dashing)
        {
            Move();
        }

        if(dashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        //animator.SetTrigger("dash");
        canDash = false;
        rigidbody2d.velocity = lastMotionVector * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        dashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * speed;
    }
}
