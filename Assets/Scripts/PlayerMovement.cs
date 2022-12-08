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

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cawSFX;
    [SerializeField] private AudioClip flySFX;
    [SerializeField] private float cawCycle;
    private float cawCycleTimer;


    private void Start()
    {
        rb.freezeRotation = true;
        groundCheckBox = new Vector3(hitbox.size.x / 2, .1f, hitbox.size.z);
        quat = new Quaternion(0, 0, 0, 0);
        cawCycleTimer = cawCycle;
    }

    private void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, groundCheckBox, quat, groundLayer);
        PlayerInput();

        //Player jump buffer
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

        //Plays caw noise on a constant cycle
        if (cawCycleTimer <= 0)
        {
            audioSource.PlayOneShot(cawSFX);
            cawCycleTimer = cawCycle;
        }
        else
        {
            cawCycleTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //Player state machine
        ControlDrag();
        MovePlayer();
        switch (state)
        {
            case State.IDLE:
                //Players is starting to move
                if (move != Vector3.zero)
                {
                    state = State.MOVING;
                    anim.SetTrigger("hopForward");
                }
                //Player is starting to jump
                else if (jumpBufferTimer > 0)
                {
                    state = State.JUMPING;
                    anim.SetTrigger("flyingTrigger");
                    Jump();
                    audioSource.PlayOneShot(flySFX);
                }
                break;

            case State.MOVING:
                //Players stopping moving
                if (move == Vector3.zero)
                {
                    state = State.IDLE;
                    anim.SetTrigger("idle");
                }
                //Player is starting to jump
                else if (jumpBufferTimer > 0)
                {
                    state = State.JUMPING;
                    Jump();
                    anim.SetTrigger("flyingTrigger");
                    audioSource.PlayOneShot(flySFX);
                }
                break;

            case State.JUMPING:
                //player is back on ground
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
        //reads in player movement
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

    void Jump()
    {
        rb.drag = airDrag;
        rb.AddForce((transform.up * upPower) + (move * forwardPower), ForceMode.Impulse);
    }
}
