using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVING,
        JUMPING
    }
    public State state;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float airDrag;
    [SerializeField] private float groundDrag; 
    private Vector3 move;
    private float forwardMovement, sidewaysMovement;
    private bool isGrounded;
    private Vector3 groundCheckBox;
    private Quaternion quat;

    [Header("Jump Stuff")]
    [SerializeField] private float upPower;
    [SerializeField] private float forwardPower;
    [SerializeField] private float jumpBuffer;
    private float jumpBufferTimer;

    [Header("Componenets/Layers")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform tf;
    [SerializeField] private BoxCollider hitbox;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;


    private void Start()
    {
        rb.freezeRotation = true;
        groundCheckBox = new Vector3(hitbox.size.x / 2, .1f, hitbox.size.z);
        quat = new Quaternion(0, 0, 0, 0);
    }

    private void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, groundCheckBox, quat, groundLayer);
        PlayerInput();
        
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("hopForward");
        }
        */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBuffer;
        }
        else
        {
            if (jumpBufferTimer >= 0)
            {
                jumpBufferTimer -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        ControlDrag();
        MovePlayer();
        switch (state)
        {
            case State.IDLE:
                if (move != Vector3.zero)
                {
                    state = State.MOVING;
                    anim.SetTrigger("hopForward");
                }
                else if (jumpBufferTimer > 0)
                {
                    state = State.JUMPING;
                    anim.SetTrigger("flyingTrigger");
                    Jump();
                }
                break;

            case State.MOVING:
                if (move == Vector3.zero)
                {
                    state = State.IDLE;
                    anim.SetTrigger("idle");
                }
                else if (jumpBufferTimer > 0)
                {
                    state = State.JUMPING;
                    Jump();
                    anim.SetTrigger("flyingTrigger");
                }
                break;

            case State.JUMPING:
                if (isGrounded)
                {
                    state = State.IDLE;
                    anim.SetTrigger("idle");                    
                }
                break;
        }
    }

    private void PlayerInput()
    {
        sidewaysMovement = Input.GetAxisRaw("Horizontal");
        forwardMovement = Input.GetAxisRaw("Vertical");
        move = tf.forward * forwardMovement + tf.right * sidewaysMovement;
        move = move.normalized;
    }

    private void MovePlayer()
    {
        rb.AddForce(move.normalized * moveSpeed, ForceMode.Acceleration);
    }

    //Updates the drag of the player based on whether they are in the air or on ground
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void updateGravity()
    {
        if (isGrounded)
        {
            
        }
    }

    void Jump()
    {
        rb.drag = airDrag;
        rb.AddForce((transform.up * upPower) + (transform.forward * forwardPower), ForceMode.Impulse);
    }
}
